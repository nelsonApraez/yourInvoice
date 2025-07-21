///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Beneficiary.Delete
{
    public sealed class DeleteBeneficiaryCommandHandler : IRequestHandler<DeleteBeneficiaryCommand, ErrorOr<bool>>
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IMoneyTransferRepository moneyTransferRepository;
        private readonly IStorage storage;
        private readonly IOfferRepository _offerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBeneficiaryCommandHandler(IDocumentRepository documentRepository, IMoneyTransferRepository moneyTransferRepository,
            IUnitOfWork unitOfWork, IStorage storage, IOfferRepository offerRepository)
        {
            this.documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            this.moneyTransferRepository = moneyTransferRepository ?? throw new ArgumentNullException(nameof(moneyTransferRepository));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<bool>> Handle(DeleteBeneficiaryCommand command, CancellationToken cancellationToken)
        {
            if (command is null || !command.beneficiaryIds.Any())
            {
                return Error.Validation(GetErrorDescription(MessageCodes.ParameterEmpty, "beneficiaryId"));
            }

            //si el estado de la oferta no es en progreso saca error
            if (!await _offerRepository.OfferIsInProgressByBeneficiaryIdAsync(command.beneficiaryIds.FirstOrDefault()))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            foreach (var beneficiaryId in command.beneficiaryIds)
            {
                if (await moneyTransferRepository.ExistsByIdAsync(beneficiaryId))
                {
                    var beneficiary = await this.moneyTransferRepository.GetByIdAsync(beneficiaryId);
                    var documents = await documentRepository.GetDocumentsByOfferAndRelatedAsync(beneficiary.OfferId, beneficiary.Id);

                    await this.documentRepository.DeleteDocumentsByOfferAndRelatedAsync(beneficiary.OfferId, beneficiary.Id);
                    await this.moneyTransferRepository.DeleteAsync(beneficiaryId);

                    foreach (var itemDoc in documents)
                    {
                        await storage.DeleteBlobByUrlAsync(itemDoc.Url);
                    }
                }
            }

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result >= 0;
        }
    }
}