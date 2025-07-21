

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;
using yourInvoice.Link.Application.LinkingProcess.EmailToAdminLinkingCompleted;
using yourInvoice.Link.Application.LinkingProcess.EmailToAdminLinkingCompletedLegal;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Accounts.Queries;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.SignSuccessDocs
{
    public sealed class SignSuccessDocsLinkCommandHandler : IRequestHandler<SignSuccessDocsLinkCommand, ErrorOr<bool>>
    {
        private readonly IZapsign _Zapsign;
        private readonly IDocumentRepository _documentRepository;
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IUnitOfWorkLink _unitOfWork;
        private readonly IMediator mediator;
        private readonly ILinkStatusRepository _linkStatusRepository;
        private readonly IAccountRepository _IAccountRepository;
        private const string LinkFormatNameSigned = "Formato de vinculación firmado.pdf";
        private const string LinkFormatName = "Formato de vinculación.pdf";
        private const string BrokerContractName = "Contrato de Corretaje Comprador.pdf";
        private const string BrokerContractNameSigned = "Contrato de Corretaje Comprador firmado.pdf";
        private const string DianRegistrationAuthorizationName = "Autorización Proveedor Tecnologico Vendedores.pdf";
        private const string DianRegistrationAuthorizationNameSigned = "Autorización Proveedor Tecnologico Vendedores firmado.pdf";
        private const string Storage = "storage";

        public SignSuccessDocsLinkCommandHandler(IUnitOfWorkLink unitOfWork, IStorage storage,  ILinkStatusRepository linkStatusRepository,
            IZapsign zapsign,  IDocumentRepository documentRepository, ICatalogBusiness catalogBusiness, 
              IMediator mediator, IAccountRepository accountRepository)
        {
            _Zapsign = zapsign ?? throw new ArgumentNullException(nameof(zapsign));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            this.mediator = mediator;
            _linkStatusRepository = linkStatusRepository ?? throw new ArgumentNullException(nameof(linkStatusRepository));
            _IAccountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<ErrorOr<bool>> Handle(SignSuccessDocsLinkCommand command, CancellationToken cancellationToken)
        {
            string formatoVinculacionUrlSignedFile = null;
            string contratoCorretajeUrlSignedFile = null;
            object urlcontratoCorretaje = null;
            string autorizacionDianUrlSignedFile = null;
            object urlautorizacionDian = null;

            var account = await _IAccountRepository.GetAccountIdAsync(command.generalInformationId);

            var document = await _documentRepository.GetAllDocumentsByRelatedIdAsync(command.generalInformationId);

            var docLinkingFormat = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.LinkingFormat && x.RelatedId == command.generalInformationId);

            var docBrokerContract = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.BrokerContract && x.RelatedId == command.generalInformationId);
            var docDianRegistrationAuthorization = document.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.DianRegistrationAuthorization && x.RelatedId == command.generalInformationId);

            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

            if (string.IsNullOrEmpty(docLinkingFormat.TokenZapsign))
            {
                //Se actualiza estado
                await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = command.generalInformationId, StatusLinkId = CatalogCodeLink_LinkStatus.SignatureUnsuccessful }, cancellationToken);
                return Error.Validation(MessageCodes.ZapsignNoToken, GetErrorDescription(MessageCodes.ZapsignNoToken));
            }

            //Obtener los documentos firmados desde zapsign
            var zapsignResponse = await _Zapsign.GetDetailAsync(docLinkingFormat.TokenZapsign);

            if (zapsignResponse.status == "pending")
            {
                //Se actualiza estado
                await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = command.generalInformationId, StatusLinkId = CatalogCodeLink_LinkStatus.SignatureUnsuccessful }, cancellationToken);
                return Error.Validation(MessageCodes.ZapsignError, GetErrorDescription(MessageCodes.ZapsignError));
            }

            //si hay algun anexo que no traiga el documento firmado se intenta traer con el token de cada uno. De lo contrario se extraen del documento principal
            if (zapsignResponse.extra_docs.Any(x => x.signed_file == null))
            {
                //Intentar obtener los documentos anexados firmados
                if (docBrokerContract != null)
                {
                    var contratoCorretaje = await _Zapsign.GetDetailAsync(zapsignResponse.extra_docs.Where(x => x.name == BrokerContractName).FirstOrDefault().token);
                    if (contratoCorretaje == null || contratoCorretaje.signed_file == null)
                    {
                        //Se actualiza estado
                        await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = command.generalInformationId, StatusLinkId = CatalogCodeLink_LinkStatus.SignatureUnsuccessful }, cancellationToken);
                        return Error.Validation(MessageCodes.ZapsignError, GetErrorDescription(MessageCodes.ZapsignError));
                    }
                    else
                    {
                        contratoCorretajeUrlSignedFile = contratoCorretaje.signed_file;
                    }
                }

                if (docDianRegistrationAuthorization != null)
                {
                    var autorizacionDian = await _Zapsign.GetDetailAsync(zapsignResponse.extra_docs.Where(x => x.name == DianRegistrationAuthorizationName).FirstOrDefault().token);
                    if (autorizacionDian == null || autorizacionDian.signed_file == null)
                    {
                        //Se actualiza estado
                        await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = command.generalInformationId, StatusLinkId = CatalogCodeLink_LinkStatus.SignatureUnsuccessful }, cancellationToken);
                        return Error.Validation(MessageCodes.ZapsignError, GetErrorDescription(MessageCodes.ZapsignError));
                    }
                    else
                    {
                        autorizacionDianUrlSignedFile = autorizacionDian.signed_file;
                    }
                }
            }
            else
            {
                //se extraen del documento principal                
                if (docBrokerContract != null)
                {
                    contratoCorretajeUrlSignedFile = zapsignResponse.extra_docs.Where(x => x.name == BrokerContractName).FirstOrDefault().signed_file;
                }
                if (docDianRegistrationAuthorization != null)
                {
                    autorizacionDianUrlSignedFile = zapsignResponse.extra_docs.Where(x => x.name == DianRegistrationAuthorizationName).FirstOrDefault().signed_file;
                }
            }

            //almacenar en el storage los documentos firmados
            string storageRute = $"{Storage}/linking/{command.generalInformationId}/";

            if (docBrokerContract != null)
            {
                urlcontratoCorretaje = await _storage.UploadAsync(await DownloadFileAsync(contratoCorretajeUrlSignedFile), storageRute + BrokerContractNameSigned);
            }
            if (docDianRegistrationAuthorization != null)
            {
                urlautorizacionDian = await _storage.UploadAsync(await DownloadFileAsync(autorizacionDianUrlSignedFile), storageRute + DianRegistrationAuthorizationNameSigned);
            }

            object urlLinkFormat = await _storage.UploadAsync(await DownloadFileAsync(zapsignResponse.signed_file), storageRute + LinkFormatNameSigned);

            //se actualizan los documentos y se actuliza el estado de la vinculacion
            await SaveInDB(docLinkingFormat, docBrokerContract, docDianRegistrationAuthorization, urlLinkFormat, urlcontratoCorretaje, urlautorizacionDian, command.generalInformationId, cancellationToken);

            await SendEmails(command, account);

            return true;
        }

        private async Task SendEmails(SignSuccessDocsLinkCommand command, AccountResponse accountResponse)
        {
            if (accountResponse.PersonTypeId == CatalogCode_PersonType.Natural)
            {
                await this.mediator.Publish(new EmailLinkingCompletedCommand(command.generalInformationId));
            }
            else
            {
                await this.mediator.Publish(new EmailLinkingCompletedLegalCommand(command.generalInformationId));
            }
        }

        private async Task SaveInDB(Document docLinkingFormat, Document docBrokerContract, Document docDianRegistrationAuthorization, object urlLinkFormat,
            object urlcontratoCorretaje, object urlautorizacionDian, Guid userId, CancellationToken cancellationToken)
        {
            //Se actualiza estado
            await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = userId, StatusLinkId = CatalogCodeLink_LinkStatus.PendingApproval }, cancellationToken);

            if (docBrokerContract != null)
            {
                docBrokerContract.IsSigned = true;
                docBrokerContract.Url = urlcontratoCorretaje.ToString();
                docBrokerContract.Name = BrokerContractNameSigned;
                _documentRepository.Update(docBrokerContract);
            }

            if (docDianRegistrationAuthorization != null)
            {
                docDianRegistrationAuthorization.IsSigned = true;
                docDianRegistrationAuthorization.Url = urlautorizacionDian.ToString();
                docDianRegistrationAuthorization.Name = DianRegistrationAuthorizationNameSigned;
                _documentRepository.Update(docDianRegistrationAuthorization);
            }

            docLinkingFormat.IsSigned = true;
            docLinkingFormat.Url = urlLinkFormat.ToString();
            docLinkingFormat.Name = LinkFormatNameSigned;

            //Se actualizan documentos
            _documentRepository.Update(docLinkingFormat);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private static async Task<byte[]> DownloadFileAsync(string urlDescarga)
        {
            using (HttpClient cliente = new())
            {
                try
                {
                    // Descargar el contenido de la URL como array de bytes
                    byte[] archivoBytes = await cliente.GetByteArrayAsync(urlDescarga);
                    return archivoBytes;
                }
                catch (HttpRequestException ex)
                {
                    // Manejar cualquier excepción de descarga aquí
#if DEBUG
                    Console.WriteLine($"Error al descargar el archivo: {ex.Message}");
#endif
                    return null;
                }
            }
        }
    }
}
