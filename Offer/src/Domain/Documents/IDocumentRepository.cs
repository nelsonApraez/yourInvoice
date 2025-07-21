///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;

namespace yourInvoice.Offer.Domain.Documents
{
    public interface IDocumentRepository
    {
        Task<List<Document>> GetDocumentsByOfferInvoiceAsync(Guid offerId, Guid invoiceId, string formatTypeFile);

        Task<bool> NullyfyDocumentsAsync(Guid offerId);

        Task<bool> DeleteDocumentsAsync(List<Guid> invoiceIds);

        Document Add(Document document);

        void Update(Document document);

        void Delete(Guid documentId);

        Task<List<Document>> GetDocumentsByOfferAndRelatedAsync(Guid offerId, Guid relatedId);

        Task<List<Document>> GetDocumentsByOfferAndTypeAsync(Guid offerId, Guid typeId);

        Task<bool> DeleteDocumentsByOfferAndRelatedAsync(Guid offerId, Guid typeId);

        Task<List<Document>> GetAllDocumentsByOfferAsync(Guid offerId);

        Task<List<Document>> GetByIdAsync(Guid documentId);

        Task<List<Document>> GetDocumentsByOfferNumberAndTypeAsync(int offerId, Guid typeId);

        Task<bool> DeleteAsync(Guid documentId);

        Task<List<ListDocsResponse>> GetOfferDocumentByTypeAsync(Guid offerId, Guid userId);
    }
}