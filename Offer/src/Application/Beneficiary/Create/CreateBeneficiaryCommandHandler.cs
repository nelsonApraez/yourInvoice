///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using System.Text;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Beneficiary.Create
{
    public class CreateBeneficiaryCommandHandler : IRequestHandler<CreateBeneficiaryCommand, ErrorOr<Guid>>
    {
        private readonly IMoneyTransferRepository repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorage _storage;
        private readonly IOfferRepository _offerRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ICatalogBusiness _catalogBusiness;
        private const string nameFileBeneficiaryDocumentOrRut = "BeneficiaryDocumentOrRut";
        private const string nameFileBeneficiaryBankCertificate = "BeneficiaryBankCertificate";

        public CreateBeneficiaryCommandHandler(IMoneyTransferRepository repository, IUnitOfWork unitOfWork,
            IStorage storage, IOfferRepository offerRepository, IDocumentRepository documentRepository, ICatalogBusiness catalogBusiness)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
        }

        public async Task<ErrorOr<Guid>> Handle(CreateBeneficiaryCommand command, CancellationToken cancellationToken)
        {
            if (await repository.ExistsByDocumentAsync(command.DocumentNumber, command.OfferId, command.BankId))
                return Error.Validation(MessageCodes.BeneficiaryExist, GetErrorDescription(MessageCodes.BeneficiaryExist));

            if (string.IsNullOrEmpty(command.BankCertificateBase64))
                return Error.Validation(MessageCodes.BankCertificateRequired, GetErrorDescription(MessageCodes.BankCertificateRequired));

            if (string.IsNullOrEmpty(command.DocumentOrRutBase64))
                return Error.Validation(MessageCodes.DocumentOrRutRequired, GetErrorDescription(MessageCodes.DocumentOrRutRequired));

            if (!IsPdf(Convert.FromBase64String(command.BankCertificateBase64)))
                return Error.Validation(MessageCodes.NoPdf, GetErrorDescription(MessageCodes.NoPdf));

            if (!IsPdf(Convert.FromBase64String(command.DocumentOrRutBase64)))
                return Error.Validation(MessageCodes.NoPdf, GetErrorDescription(MessageCodes.NoPdf));

            Domain.Offer offer = await _offerRepository.GetByIdAsync(command.OfferId);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            //si el estado de la oferta no es en progreso saca error
            if (!await _offerRepository.OfferIsInProgressAsync(command.OfferId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            var totalOffer = await _offerRepository.TotalOfferAsync(command.OfferId);
            var totalBeneficiaries = await repository.TotalAsync(command.OfferId);
            totalBeneficiaries += command.Total;

            if (totalBeneficiaries > totalOffer)            
                return Error.Validation(MessageCodes.HigherAmount, GetErrorDescription(MessageCodes.HigherAmount,totalOffer.Value.ToString("C0", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"))));            

            int count = await repository.GetCountId(command.OfferId);
            var maxBeneficiaries = await _catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.MaxBeneficiaries);

            if (count >= Convert.ToInt16(maxBeneficiaries.Descripton))
                return Error.Validation(MessageCodes.Beneficiaries10, GetErrorDescription(MessageCodes.Beneficiaries10));

            var pdfBankCertificate = Convert.FromBase64String(command.BankCertificateBase64);
            var pdfDocumentOrRut = Convert.FromBase64String(command.DocumentOrRutBase64);

            string storageRute = "storage/" + offer.Consecutive + "/Documents/Beneficiaries/" + command.DocumentNumber + "/";

            var countBeneficiary = await repository.CountBeneficiaryAsync(command.DocumentNumber, command.OfferId);
            var naemeBeneficiaryDocumentOrRut = countBeneficiary <= 0 ? $"{nameFileBeneficiaryDocumentOrRut}.pdf" : $"{nameFileBeneficiaryDocumentOrRut}_{countBeneficiary}.pdf";
            var naemeBeneficiaryBankCertificate = countBeneficiary <= 0 ? $"{nameFileBeneficiaryBankCertificate}.pdf" : $"{nameFileBeneficiaryBankCertificate}_{countBeneficiary}.pdf";

            object urlDocumentOrRut = await _storage.UploadAsync(pdfDocumentOrRut, storageRute + naemeBeneficiaryDocumentOrRut);
            object urlBankCertificate = await _storage.UploadAsync(pdfBankCertificate, storageRute + naemeBeneficiaryBankCertificate);

            MoneyTransfer moneyTransfer = await SaveInDB(command, urlDocumentOrRut, urlBankCertificate, naemeBeneficiaryDocumentOrRut, naemeBeneficiaryBankCertificate, cancellationToken);

            return moneyTransfer.Id;
        }

        private async Task<MoneyTransfer> SaveInDB(CreateBeneficiaryCommand command, object urlDocumentOrRut, object urlBankCertificate,
            string naemeBeneficiaryDocumentOrRut, string naemeBeneficiaryBankCertificate, CancellationToken cancellationToken)
        {
            MoneyTransfer moneyTransfer = new(Guid.NewGuid(), command.OfferId, command.DocumentTypeId, command.DocumentNumber, command.BankId,
                command.AccountTypeId, command.AccountNumber, command.Total, command.Name, command.PersonTypeId);

            Document DocumentOrRut = new(Guid.NewGuid(), command.OfferId, moneyTransfer.Id, naemeBeneficiaryDocumentOrRut, CatalogCode_DocumentType.BeneficiaryDocumentOrRut, false, urlDocumentOrRut.ToString());
            Document BankCertificate = new(Guid.NewGuid(), command.OfferId, moneyTransfer.Id, naemeBeneficiaryBankCertificate, CatalogCode_DocumentType.BeneficiaryBankCertificate, false, urlBankCertificate.ToString());

            _documentRepository.Add(DocumentOrRut);
            _documentRepository.Add(BankCertificate);

            repository.Add(moneyTransfer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return moneyTransfer;
        }

        private bool IsPdf(byte[] bytes)
        {
            // La firma de un archivo PDF está formada por los primeros cuatro bytes: %PDF
            byte[] pdfSignature = Encoding.ASCII.GetBytes("%PDF");

            for (int i = 0; i < pdfSignature.Length; i++)
            {
                if (bytes.Length <= i || bytes[i] != pdfSignature[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}