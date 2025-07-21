///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GetDocumentsByRelatedId
{
    public sealed class GetDocumentsByRelatedIdQueryHandler : IRequestHandler<GetDocumentsByRelatedIdQuery, ErrorOr<List<GetDocumentResponse>>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IDocumentRepository _documentRepository;

        public GetDocumentsByRelatedIdQueryHandler(IAccountRepository accountRepository, IDocumentRepository documentRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        }

        public async Task<ErrorOr<List<GetDocumentResponse>>> Handle(GetDocumentsByRelatedIdQuery request, CancellationToken cancellationToken)
        {
            List<GetDocumentResponse> listDocuments = new List<GetDocumentResponse>();

            var account = await _accountRepository.GetAccountIdAsync(request.relatedId);

            bool isNatural = account.PersonTypeId.Equals(CatalogCode_PersonType.Natural);
            bool isSeller = account.RoleId.Equals(CatalogCode_UserRole.Seller);

            var documents = await _documentRepository.GetAllDocumentsByRelatedIdAsync(request.relatedId);

            #region LINKING FORMAT
            var document = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.LinkingFormat);
            if (document == null)
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));
            else
                listDocuments.Add(new GetDocumentResponse
                {
                    DocumentId = document.Id,
                    Name = document.Name,
                    IsSigned = (bool)document.IsSigned,
                    Size = document.FileSize
                });
            #endregion

            #region BROKER CONTRACT
            document = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.BrokerContract);
            if (!isSeller && document == null)
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));
            else if (!isSeller && document != null)
                listDocuments.Add(new GetDocumentResponse
                {
                    DocumentId = document.Id,
                    Name = document.Name,
                    IsSigned = (bool)document.IsSigned,
                    Size = document.FileSize
                });
            #endregion

            #region REGISTRATION AUTHORIZATION
            document = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.DianRegistrationAuthorization);
            if (isSeller && !isNatural && document == null)
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));
            else if (isSeller && !isNatural && document != null)
                listDocuments.Add(new GetDocumentResponse
                {
                    DocumentId = document.Id,
                    Name = document.Name,
                    IsSigned = (bool)document.IsSigned,
                    Size = document.FileSize
                });
            #endregion

            return listDocuments;
        }
    }
}
