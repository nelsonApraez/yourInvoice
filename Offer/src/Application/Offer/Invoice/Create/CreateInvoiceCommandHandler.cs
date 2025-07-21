///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Invoice.Create
{
    public sealed class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, ErrorOr<Guid>>
    {
        private readonly IInvoiceRepository repository;
        private readonly IOfferRepository _offerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateInvoiceCommandHandler(IInvoiceRepository repository, IOfferRepository offerRepository, IUnitOfWork unitOfWork)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<Guid>> Handle(CreateInvoiceCommand command, CancellationToken cancellationToken)
        {
            //si el estado de la oferta no es en progreso saca error
            if (!await _offerRepository.OfferIsInProgressAsync(command.OfferId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            var invoice = new Domain.Invoices.Invoice(
                    command.Id,
                    command.OfferId,
                    command.Number,
                    command.ZipName,
                    command.Cufe,
                    command.StatusId,
                    command.EmitDate,
                    command.DueDate,
                    command.Total,
                    command.TotalAmount,
                    command.MoneyTypeId,
                    command.Trm,
                    command.ErrorMessage,
                    command.NegotiationDate,
                    command.NegotiationTotal
            );

            this.repository.Add(invoice);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return invoice.Id;
        }
    }
}