///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Link.Domain.Document;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly LinkDbContext _context;

        public DocumentRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Document Add(Document document)
        {
            return _context.Documents.Add(document).Entity;
        }

        public async Task<List<Document>> GetByIdAsync(Guid documentId)
        {
            var result = await _context.Documents.Where(c => c.Id == documentId).ToListAsync();
            return result;
        }

        public async Task<List<Document>> GetAllDocumentsByRelatedIdAsync(Guid relatedId)
        {
            var result = await _context.Documents.Where(c => c.RelatedId == relatedId).ToListAsync();
            return result;
        }               

        public void Update(Document document)
        {
            _context.Documents.Update(document);
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            await _context.Documents.Where(c => c.Id == documentId).ExecuteDeleteAsync();
            return true;
        }


    }
}