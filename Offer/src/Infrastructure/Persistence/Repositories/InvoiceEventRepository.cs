///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.InvoiceEvents;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class InvoiceEventRepository : IInvoiceEventRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceEventRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(InvoiceEvent invoiceEvent)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid invoiceEventId)
        {
            throw new NotImplementedException();
        }

        public void Update(InvoiceEvent invoiceEvent)
        {
            throw new NotImplementedException();
        }

        public async Task NullyfyAsync(Guid offerId)
        {
            var invoiceIds = (from I in _context.Invoices
                              join E in _context.InvoiceEvents on I.Id equals E.InvoiceId
                              where I.OfferId == offerId
                              select E.InvoiceId).ToList();
            await _context.InvoiceEvents.Where(x => invoiceIds.Contains(x.InvoiceId)).ExecuteDeleteAsync();
        }

        public async Task DeleteAsync(List<Guid> invoiceIds)
        {
            await _context.InvoiceEvents.Where(x => invoiceIds.Contains(x.InvoiceId)).ExecuteDeleteAsync();
        }
    }
}