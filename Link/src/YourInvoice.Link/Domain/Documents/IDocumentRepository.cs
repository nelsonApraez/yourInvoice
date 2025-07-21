namespace yourInvoice.Link.Domain.Document
{
    public interface IDocumentRepository
    {
        Document Add(Document document);

        void Update(Document document);

        Task<List<Document>> GetAllDocumentsByRelatedIdAsync(Guid relatedId);

        Task<List<Document>> GetByIdAsync(Guid documentId);

        Task<bool> DeleteAsync(Guid documentId);
    }
}