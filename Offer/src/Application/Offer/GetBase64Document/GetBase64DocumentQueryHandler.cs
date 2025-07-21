///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.GetBase64Document
{
    public sealed class GetBase64DocumentQueryHandler : IRequestHandler<GetBase64DocumentQuery, ErrorOr<string[]>>
    {
        private readonly IDocumentRepository _repository;
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;

        public GetBase64DocumentQueryHandler(IDocumentRepository repository, IStorage storage, ICatalogBusiness catalogBusiness)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
        }

        public async Task<ErrorOr<string[]>> Handle(GetBase64DocumentQuery query, CancellationToken cancellationToken)
        {
            var doc = await _repository.GetByIdAsync(query.documentId);
            if (!doc.Any())
                return Error.Validation(MessageCodes.DocumentNotExist, GetErrorDescription(MessageCodes.DocumentNotExist));

            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

            string blobName = GetBlobNameFromUrl(doc.FirstOrDefault().Url, containerName.Descripton);

            var docMs = await _storage.DownloadAsync(blobName);

            string[] result = new string[2];
            result[0] = Convert.ToBase64String(docMs.ToArray());
            result[1] = TransformModule.GetFileExt(doc.FirstOrDefault().Name);
            return result;
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