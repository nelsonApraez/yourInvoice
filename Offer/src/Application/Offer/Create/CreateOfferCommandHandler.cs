///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.HistoricalStates.Add;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Create
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, ErrorOr<CreateOfferResponse>>
    {
        private readonly IOfferRepository repository;
        private readonly IPayerRepository payerRepository;
        private readonly IMediator mediator;
        private readonly ISystem system;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOfferCommandHandler(IOfferRepository repository, IUnitOfWork unitOfWork, IPayerRepository payerRepository, IMediator mediator, ISystem system)
        {
            this.payerRepository = payerRepository ?? throw new ArgumentNullException(nameof(payerRepository));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<CreateOfferResponse>> Handle(CreateOfferCommand command, CancellationToken cancellationToken)
        {
            var guidSeller = this.system.User.Id;
            if (await payerRepository.GetByIdAsync(command.PayerId) == null)
                return Error.Validation(MessageCodes.PayerNotExist, GetErrorDescription(MessageCodes.PayerNotExist));

            Domain.Offer offer = new(Guid.NewGuid(), command.PayerId, guidSeller, null, null, null, CatalogCode_OfferStatus.InProgress);

            repository.Add(offer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await this.mediator.Publish(new AddHistoricalCommand { OfferId = offer.Id, StatusId = CatalogCode_OfferStatus.InProgress, UserId = offer.UserId }, cancellationToken);

            return new CreateOfferResponse { Consecutive = offer.Consecutive, OfferId = offer.Id };
        }
    }
}