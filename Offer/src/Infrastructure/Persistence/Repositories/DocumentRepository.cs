///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISystem system;

        public DocumentRepository(ApplicationDbContext context, ISystem system) : base(context, system)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.system = system;
        }

        public Document Add(Document document)
        {
            return base.Create(document);
        }

        public void Delete(Guid documentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Document>> GetDocumentsByOfferInvoiceAsync(Guid offerId, Guid invoiceId, string formatTypeFile)
        {
            var result = await _context.Documents.Where(c => c.OfferId == offerId && c.RelatedId == invoiceId && c.Name.Trim().EndsWith(formatTypeFile)).ToListAsync();
            return result;
        }

        public async Task<List<Document>> GetDocumentsByOfferAndTypeAsync(Guid offerId, Guid typeId)
        {
            var result = await _context.Documents.Where(c => c.OfferId == offerId && c.TypeId == typeId).ToListAsync();
            return result;
        }

        public async Task<List<Document>> GetByIdAsync(Guid documentId)
        {
            var result = await _context.Documents.Where(c => c.Id == documentId).ToListAsync();
            return result;
        }

        public async Task<List<Document>> GetDocumentsByOfferAndRelatedAsync(Guid offerId, Guid relatedId)
        {
            var result = await _context.Documents.Where(c => c.OfferId == offerId && c.RelatedId == relatedId).ToListAsync();
            return result;
        }

        public async Task<List<Document>> GetAllDocumentsByOfferAsync(Guid offerId)
        {
            var result = await _context.Documents.Where(c => c.OfferId == offerId).ToListAsync();
            return result;
        }

        public async Task<bool> NullyfyDocumentsAsync(Guid offerId)
        {
            var result = await _context.Documents.Where(c => c.OfferId == offerId).ExecuteDeleteAsync();
            return result >= 0;
        }

        public async Task<bool> DeleteDocumentsAsync(List<Guid> invoiceIds)
        {
            var result = await _context.Documents.Where(x => invoiceIds.Contains((Guid)x.RelatedId)).ExecuteDeleteAsync();
            return result >= 0;
        }

        public async Task<bool> DeleteDocumentsByOfferAndRelatedAsync(Guid offerId, Guid relatedId)
        {
            var result = await _context.Documents.Where(x => x.OfferId == offerId && x.RelatedId == relatedId).ExecuteDeleteAsync();
            return result >= 0;
        }

        public void Update(Document document)
        {
            _context.Documents.Update(document);
        }

        public async Task<List<Document>> GetDocumentsByOfferNumberAndTypeAsync(int offerId, Guid typeId)
        {
            var result = await (from D in _context.Documents
                                join O in _context.Offers on D.OfferId equals O.Id
                                where O.Consecutive == offerId && D.TypeId == typeId
                                select D).ToListAsync();
            return result;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            await _context.Documents.Where(c => c.Id == documentId).ExecuteDeleteAsync();
            return true;
        }

        public async Task<List<ListDocsResponse>> GetOfferDocumentByTypeAsync(Guid offerId, Guid userId)
        {
            var result = await (from O in _context.Offers
                                join D in _context.Documents on O.Id equals D.OfferId
                                where O.UserId == userId && O.Id == offerId && (O.StatusId == CatalogCode_OfferStatus.InProgress || O.StatusId == CatalogCode_OfferStatus.Enabled || O.StatusId == CatalogCode_OfferStatus.Purchased)
                                && (D.TypeId == CatalogCode_DocumentType.MoneyTransferInstruction || D.TypeId == CatalogCode_DocumentType.CommercialOffer
                                || D.TypeId == CatalogCode_DocumentType.Endorsement || D.TypeId == CatalogCode_DocumentType.EndorsementNotification) && D.Name.Contains(".pdf")
                                select new ListDocsResponse
                                {
                                    DocumentId = D.Id,
                                    Name = D.Name,
                                    IsSigned = D.IsSigned ?? false,
                                    Size = D.FileSize,
                                }).Distinct().ToListAsync();

            return result;
        }
    }
}