///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Offer.Application.Offer.SignDocs;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.SignDocs
{
    public sealed class SignDocsBuyerCommandHandler : IRequestHandler<SignDocsBuyerCommand, ErrorOr<SignDocsResponse>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IZapsign _Zapsign;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceDispersionRepository _invoiceDispersionRepository;
        private readonly ISystem system;

        public SignDocsBuyerCommandHandler(IUnitOfWork unitOfWork, IStorage storage, IOfferRepository offerRepository,
            IZapsign zapsign, IUserRepository userRepository, IDocumentRepository documentRepository, ICatalogBusiness catalogBusiness,
            IInvoiceDispersionRepository invoiceDispersionRepository, ISystem system)
        {
            _Zapsign = zapsign ?? throw new ArgumentNullException(nameof(zapsign));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            _invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
            this.system = system;
        }

        public async Task<ErrorOr<SignDocsResponse>> Handle(SignDocsBuyerCommand command, CancellationToken cancellationToken)
        {
            var buyerId = this.system.User.Id;
            Domain.Offer offer = await _offerRepository.GetByConsecutiveAsync(command.numberOffer);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            //si el estado de la oferta es comprada saca error
            bool IsPurchased = await _offerRepository.OfferIsPurchasedAsync(command.numberOffer);
            if (IsPurchased)
                return Error.Validation(MessageCodes.MessageOfferIsPurchased, GetErrorDescription(MessageCodes.MessageOfferIsPurchased));

            var invoiceDisper = await _invoiceDispersionRepository.GetByOfferNumberAndBuyerIdAsync(command.numberOffer, buyerId);

            if (invoiceDisper == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var document = await _documentRepository.GetAllDocumentsByOfferAsync(offer.Id);

            var docCommercialOfferBuyer = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOfferBuyer && x.RelatedId == buyerId);
            var docPurchaseCertificate = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.PurchaseCertificate && x.RelatedId == buyerId);
            var docMoneyTransferInstructionBuyer = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstructionBuyer && x.RelatedId == buyerId);

            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

            string blobNamePurchaseCertificate = GetBlobNameFromUrl(docPurchaseCertificate.Url, containerName.Descripton);
            MemoryStream docPurchaseCertificateMs = await _storage.DownloadAsync(blobNamePurchaseCertificate);

            string blobNameCommercialOfferBuyer = GetBlobNameFromUrl(docCommercialOfferBuyer.Url, containerName.Descripton);
            MemoryStream docCommercialOfferBuyerMs = await _storage.DownloadAsync(blobNameCommercialOfferBuyer);

            MemoryStream docMoneyTransferInstructionBuyerMs = null;
            if (docMoneyTransferInstructionBuyer != null)
            {
                string blobNameMoneyTransferInstructionBuyer = GetBlobNameFromUrl(docMoneyTransferInstructionBuyer.Url, containerName.Descripton);
                docMoneyTransferInstructionBuyerMs = await _storage.DownloadAsync(blobNameMoneyTransferInstructionBuyer);
            }

            var user = await _userRepository.GetByIdAsync(invoiceDisper.BuyerId);

            ZapsignFileResponse response = await SendDocsToZapsign(docPurchaseCertificate, docMoneyTransferInstructionBuyer, docCommercialOfferBuyer,
                docPurchaseCertificateMs, docMoneyTransferInstructionBuyerMs, docCommercialOfferBuyerMs, user);

            if (response.status != "pending" || string.IsNullOrEmpty(response.token))
            {
                return Error.Validation(MessageCodes.ZapsignNoToken, GetErrorDescription(MessageCodes.ZapsignNoToken));
            }

            //se actualiza el documento principal con el token que respondio zapsign
            docCommercialOfferBuyer.TokenZapsign = response.token;
            _documentRepository.Update(docCommercialOfferBuyer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SignDocsResponse { Token = response.signers.FirstOrDefault().token, Url = response.signers.FirstOrDefault().sign_url };
        }

        private async Task<ZapsignFileResponse> SendDocsToZapsign(Document docPurchaseCertificate, Document docMoneyTransferInstructionBuyer, Document docCommercialOfferBuyer,
          MemoryStream docPurchaseCertificateMs, MemoryStream docMoneyTransferInstructionBuyerMs, MemoryStream docCommercialOfferBuyerMs, Domain.Users.User user)
        {
            ZapsignSignerRequest zapsignSignerRequest = new()
            {
                name = user.Name,
                email = user.Email
            };

            ZapsignFileRequest zapsignFileRequest = new()
            {
                name = docCommercialOfferBuyer.Name,
                base64_pdf = Convert.ToBase64String(docCommercialOfferBuyerMs.ToArray()),
                externalId = docCommercialOfferBuyer.Id.ToString(),
                Signers = new List<ZapsignSignerRequest> { zapsignSignerRequest }
            };

            var response = await _Zapsign.CreateDocAsync(zapsignFileRequest);

            ZapsignFileAttachmentRequest zapsignFileAttachmentRequestPurchaseCertificate = new()
            {
                base64_pdf = Convert.ToBase64String(docPurchaseCertificateMs.ToArray()),
                name = docPurchaseCertificate.Name
            };
            await _Zapsign.AddAttachmentAsync(response.token, zapsignFileAttachmentRequestPurchaseCertificate);

            if (docMoneyTransferInstructionBuyer != null)
            {
                ZapsignFileAttachmentRequest zapsignFileAttachmentRequestInstruccionGiroComprador = new()
                {
                    base64_pdf = Convert.ToBase64String(docMoneyTransferInstructionBuyerMs.ToArray()),
                    name = docMoneyTransferInstructionBuyer.Name
                };
                await Task.Delay(TimeSpan.FromSeconds(1));// Se da 1 segundos de espera antes de adjuntar el otro documento, sino saca error.
                await _Zapsign.AddAttachmentAsync(response.token, zapsignFileAttachmentRequestInstruccionGiroComprador);
            }

            return response;
        }

        private static string GetBlobNameFromUrl(string url, string container)
        {
            // Busca la posición de la subcadena en el texto
            int indice = url.IndexOf(container);

            // Si la subcadena no se encuentra, devuelve el texto completo
            if (indice == -1)
            {
                return url;
            }

            // Utiliza Substring para obtener la parte derecha basada en la posición de la subcadena
            return url.Substring(indice + container.Length);
        }
    }
}