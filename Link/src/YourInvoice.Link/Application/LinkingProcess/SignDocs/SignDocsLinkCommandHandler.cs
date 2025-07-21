using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;

namespace yourInvoice.Link.Application.LinkingProcess.SignDocs
{
    public sealed class SignDocsLinkCommandHandler : IRequestHandler<SignDocsLinkCommand, ErrorOr<SignDocsResponse>>
    {
        private readonly IZapsign _Zapsign;
        private readonly IDocumentRepository _documentRepository;
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IUnitOfWorkLink _unitOfWork;
        private readonly ILinkStatusRepository _linkStatusRepository;
        private readonly IAccountRepository _IAccountRepository;
        private readonly IMediator mediator;

        public SignDocsLinkCommandHandler(IUnitOfWorkLink unitOfWork, IStorage storage,
            IZapsign zapsign, IDocumentRepository documentRepository, ICatalogBusiness catalogBusiness
            , ILinkStatusRepository linkStatusRepository, IAccountRepository accountRepository, IMediator mediator)
        {
            _Zapsign = zapsign ?? throw new ArgumentNullException(nameof(zapsign));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            _linkStatusRepository = linkStatusRepository ?? throw new ArgumentNullException(nameof(linkStatusRepository));
            _IAccountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            this.mediator = mediator;
        }

        public async Task<ErrorOr<SignDocsResponse>> Handle(SignDocsLinkCommand command, CancellationToken cancellationToken)
        {
            string name, email = null;

            LinkStatus statusId = await _linkStatusRepository.GetLinkStatusAsync(command.generalInformationId);

            if (statusId.StatusLinkId == CatalogCodeLink_LinkStatus.PendingApproval)
                return Error.Validation(MessageCodes.PendigApproval, GetErrorDescription(MessageCodes.PendigApproval));

            var account = await _IAccountRepository.GetAccountIdAsync(command.generalInformationId);

            name = account.Name + " " + account.SecondName + " " + account.LastName + " " + account.SecondLastName;
            email = account.Email;

            var document = await _documentRepository.GetAllDocumentsByRelatedIdAsync(command.generalInformationId);

            var docLinkingFormat = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.LinkingFormat && x.RelatedId == command.generalInformationId);//DOC Principal

            var docBrokerContract = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.BrokerContract && x.RelatedId == command.generalInformationId);
            var docDianRegistrationAuthorization = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.DianRegistrationAuthorization && x.RelatedId == command.generalInformationId);

            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

            MemoryStream docBrokerContractMs = null;
            if (docBrokerContract != null)
            {
                string blobNameBrokerContract = GetBlobNameFromUrl(docBrokerContract.Url, containerName.Descripton);
                docBrokerContractMs = await _storage.DownloadAsync(blobNameBrokerContract);
            }

            MemoryStream docDianRegistrationAuthorizationMs = null;
            if (docDianRegistrationAuthorization != null)
            {
                string blobNameDianRegistrationAuthorization = GetBlobNameFromUrl(docDianRegistrationAuthorization.Url, containerName.Descripton);
                docDianRegistrationAuthorizationMs = await _storage.DownloadAsync(blobNameDianRegistrationAuthorization);
            }            

            string blobNameLinkingFormat = GetBlobNameFromUrl(docLinkingFormat.Url, containerName.Descripton);
            MemoryStream docLinkingFormatMs = await _storage.DownloadAsync(blobNameLinkingFormat);

            ZapsignFileResponse response = await SendDocsToZapsign(docLinkingFormat,
                docLinkingFormatMs, name, email, docBrokerContract, docBrokerContractMs, docDianRegistrationAuthorization, docDianRegistrationAuthorizationMs);

            if (response.status != "pending" || string.IsNullOrEmpty(response.token))
            {
                //Se actualiza estado
                await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = command.generalInformationId, StatusLinkId = CatalogCodeLink_LinkStatus.SignatureUnsuccessful }, cancellationToken);
                return Error.Validation(MessageCodes.ZapsignNoToken, GetErrorDescription(MessageCodes.ZapsignNoToken));
            }

            //se actualiza el documento principal con el token que respondio zapsign
            docLinkingFormat.TokenZapsign = response.token;
            _documentRepository.Update(docLinkingFormat);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SignDocsResponse { Token = response.signers.FirstOrDefault().token, Url = response.signers.FirstOrDefault().sign_url };
        }

        private async Task<ZapsignFileResponse> SendDocsToZapsign(Document docLinkingFormat,
          MemoryStream docLinkingFormatMs, string name, string email, Document docBrokerContract,
          MemoryStream docBrokerContractMs, Document docDianRegistrationAuthorization, MemoryStream docDianRegistrationAuthorizationMs)
        {
            ZapsignSignerRequest zapsignSignerRequest = new()
            {
                name = name,
                email = email
            };

            ZapsignFileRequest zapsignFileRequest = new()
            {
                name = docLinkingFormat.Name,
                base64_pdf = Convert.ToBase64String(docLinkingFormatMs.ToArray()),
                externalId = docLinkingFormat.Id.ToString(),
                Signers = new List<ZapsignSignerRequest> { zapsignSignerRequest }
            };

            var response = await _Zapsign.CreateDocAsync(zapsignFileRequest);


            if (docBrokerContract != null)
            {
                ZapsignFileAttachmentRequest zapsignFileAttachmentRequestdocBrokerContract = new()
                {
                    base64_pdf = Convert.ToBase64String(docBrokerContractMs.ToArray()),
                    name = docBrokerContract.Name
                };
                await Task.Delay(TimeSpan.FromSeconds(1));// Se da 1 segundos de espera antes de adjuntar el otro documento, sino saca error.
                await _Zapsign.AddAttachmentAsync(response.token, zapsignFileAttachmentRequestdocBrokerContract);
            }

            if (docDianRegistrationAuthorization != null)
            {
                ZapsignFileAttachmentRequest zapsignFileAttachmentRequestDianRegistrationAuthorization = new()
                {
                    base64_pdf = Convert.ToBase64String(docDianRegistrationAuthorizationMs.ToArray()),
                    name = docDianRegistrationAuthorization.Name
                };
                await _Zapsign.AddAttachmentAsync(response.token, zapsignFileAttachmentRequestDianRegistrationAuthorization);
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
