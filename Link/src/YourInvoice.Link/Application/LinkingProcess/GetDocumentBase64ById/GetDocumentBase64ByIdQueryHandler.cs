///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Link.Domain.Document;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GetDocumentBase64ById
{
    public sealed class GetDocumentBase64ByIdQueryHandler : IRequestHandler<GetDocumentBase64ByIdQuery, ErrorOr<string[]>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IStorage _storage;

        public GetDocumentBase64ByIdQueryHandler(IDocumentRepository documentRepository, ICatalogBusiness catalogBusiness, IStorage storage)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public async Task<ErrorOr<string[]>> Handle(GetDocumentBase64ByIdQuery request, CancellationToken cancellationToken)
        {
            var document = await _documentRepository.GetByIdAsync(request.idDocument);
            if (document == null || !document.Any())
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));

            var container = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);
            string blobName = GetBlobNameFromUrl(document.FirstOrDefault().Url, container.Descripton);

            var documentStorage = await _storage.DownloadAsync(blobName);

            string[] result = new string[2];
            result[0] = Convert.ToBase64String(documentStorage.ToArray());
            result[1] = TransformModule.GetFileExt(document.FirstOrDefault().Name);

            return result;
        }

        private string GetBlobNameFromUrl(string url, string container)
        {
            int indice = url.IndexOf(container);
            if (indice == -1)
                return url;
            
            return url.Substring(indice + container.Length);
        }
    }
}
