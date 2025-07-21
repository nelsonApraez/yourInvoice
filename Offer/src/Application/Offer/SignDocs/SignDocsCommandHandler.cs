///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.SignDocs
{
    public sealed class SignDocsCommandHandler : IRequestHandler<SignDocsCommand, ErrorOr<SignDocsResponse>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IZapsign _Zapsign;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IUnitOfWork _unitOfWork;

        public SignDocsCommandHandler(IUnitOfWork unitOfWork, IStorage storage, IOfferRepository offerRepository,
            IZapsign zapsign, IUserRepository userRepository, IDocumentRepository documentRepository, ICatalogBusiness catalogBusiness)
        {
            _Zapsign = zapsign ?? throw new ArgumentNullException(nameof(zapsign));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
        }

        public async Task<ErrorOr<SignDocsResponse>> Handle(SignDocsCommand command, CancellationToken cancellationToken)
        {
            Domain.Offer offer = await _offerRepository.GetByIdAsync(command.offerId);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            //si el estado de la oferta no es en progreso saca error
            if (!await _offerRepository.OfferIsInProgressAsync(command.offerId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            var docs = await _documentRepository.GetAllDocumentsByOfferAsync(command.offerId);

            //var docAppendix = docs.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Appendix);
            var docMoneyTransferInstruction = docs.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstruction);
            var docCommercialOffer = docs.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOffer);
            var docEndorsement = docs.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Endorsement);
            var docEndorsementNotification = docs.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.EndorsementNotification);

            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

            //string blobNameAppendix = GetBlobNameFromUrl(docAppendix.Url, containerName.Descripton);
            //var docAppendixMs = await _storage.DownloadAsync(blobNameAppendix);

            string blobNameMoneyTransferInstruction = GetBlobNameFromUrl(docMoneyTransferInstruction.Url, containerName.Descripton);
            var docMoneyTransferInstructionMs = await _storage.DownloadAsync(blobNameMoneyTransferInstruction);

            string blobNameCommercialOffer = GetBlobNameFromUrl(docCommercialOffer.Url, containerName.Descripton);
            var docCommercialOfferMs = await _storage.DownloadAsync(blobNameCommercialOffer);

            string blobNameEndorsement = GetBlobNameFromUrl(docEndorsement.Url, containerName.Descripton);
            var docEndorsementMs = await _storage.DownloadAsync(blobNameEndorsement);

            string blobNameEndorsementNotification = GetBlobNameFromUrl(docEndorsementNotification.Url, containerName.Descripton);
            var docEndorsementNotificationMs = await _storage.DownloadAsync(blobNameEndorsementNotification);

            var user = await _userRepository.GetByIdAsync(offer.UserId);

            ZapsignFileResponse response = await SendDocsToZapsign(docMoneyTransferInstruction, docCommercialOffer, docEndorsement, docEndorsementNotification,
                docMoneyTransferInstructionMs, docCommercialOfferMs, docEndorsementMs, docEndorsementNotificationMs, user);

            if (response.status != "pending" || string.IsNullOrEmpty(response.token))
            {
                return Error.Validation(MessageCodes.ZapsignNoToken, GetErrorDescription(MessageCodes.ZapsignNoToken));
            }

            //se actualiza el documento principal con el token que respondio zapsign
            docCommercialOffer.TokenZapsign = response.token;
            _documentRepository.Update(docCommercialOffer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SignDocsResponse { Token = response.signers.FirstOrDefault().token, Url = response.signers.FirstOrDefault().sign_url };
        }

        private async Task<ZapsignFileResponse> SendDocsToZapsign(Document docMoneyTransferInstruction, Document docCommercialOffer,
          Document docEndorsement, Document docEndorsementNotification, MemoryStream docMoneyTransferInstructionMs,
          MemoryStream docCommercialOfferMs, MemoryStream docEndorsementMs, MemoryStream docEndorsementNotificationMs, Domain.Users.User user)
        {
            ZapsignSignerRequest zapsignSignerRequest = new()
            {
                name = user.Name,
                email = user.Email
            };

            ZapsignFileRequest zapsignFileRequest = new()
            {
                name = docCommercialOffer.Name,
                base64_pdf = Convert.ToBase64String(docCommercialOfferMs.ToArray()),
                externalId = docCommercialOffer.Id.ToString(),
                Signers = new List<ZapsignSignerRequest> { zapsignSignerRequest }
            };

            var response = await _Zapsign.CreateDocAsync(zapsignFileRequest);

            //ZapsignFileAttachmentRequest zapsignFileAttachmentRequestAnexo = new()
            //{
            //    base64_pdf = Convert.ToBase64String(docAppendixMs.ToArray()),
            //    name = docAppendix.Name
            //};
            //await _Zapsign.AddAttachmentAsync(response.token, zapsignFileAttachmentRequestAnexo);

            ZapsignFileAttachmentRequest zapsignFileAttachmentRequestInstruccionGiro = new()
            {
                base64_pdf = Convert.ToBase64String(docMoneyTransferInstructionMs.ToArray()),
                name = docMoneyTransferInstruction.Name
            };
            await Task.Delay(TimeSpan.FromSeconds(1));// Se da 1 segundos de espera antes de adjuntar el otro documento, sino saca error.
            await _Zapsign.AddAttachmentAsync(response.token, zapsignFileAttachmentRequestInstruccionGiro);

            ZapsignFileAttachmentRequest zapsignFileAttachmentRequestEndorsement = new()
            {
                base64_pdf = Convert.ToBase64String(docEndorsementMs.ToArray()),
                name = docEndorsement.Name
            };
            await Task.Delay(TimeSpan.FromSeconds(1));// Se da 1 segundos de espera antes de adjuntar el otro documento, sino saca error.
            await _Zapsign.AddAttachmentAsync(response.token, zapsignFileAttachmentRequestEndorsement);

            ZapsignFileAttachmentRequest zapsignFileAttachmentRequestEndorsementNoti = new()
            {
                base64_pdf = Convert.ToBase64String(docEndorsementNotificationMs.ToArray()),
                name = docEndorsementNotification.Name
            };
            await Task.Delay(TimeSpan.FromSeconds(1));// Se da 1 segundos de espera antes de adjuntar el otro documento, sino saca error.
            await _Zapsign.AddAttachmentAsync(response.token, zapsignFileAttachmentRequestEndorsementNoti);

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