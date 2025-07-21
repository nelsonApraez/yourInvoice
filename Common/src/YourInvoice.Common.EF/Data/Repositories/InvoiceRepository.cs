///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;

namespace yourInvoice.Common.EF.Data.Repositories
{
    public class InvoiceRepository : Repository<InvoiceInfo>, IInvoiceRepository
    {
        private yourInvoiceCommonDbContext _db;

        public InvoiceRepository(yourInvoiceCommonDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public async Task<bool> UpdateStateInvoiceAsync(List<InvoiceInfo> invoiceIds)
        {
#warning PENDIENTE CAMBIAR EL VALOR DE ESTADO QUEMADO, POR EL MOMENTO SE VAN A APROBAR TODAS.
            foreach (var invoice in invoiceIds)
            {
                await _db.Invoices.Where(c => c.Id == invoice.Id)
                                    .ExecuteUpdateAsync(p => p
                                    .SetProperty(u => u.StatusId, invoice.StatusId)// CatalogCode_InvoiceStatus.Approved)  //invoice.StatusId
                                    .SetProperty(u => u.ErrorMessage, invoice.ErrorMessage ?? string.Empty)
                                    .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO()));
            }

            return true;
        }

        public async Task<bool> UpdateStateInvoiceProcessDianAsync(Guid offerId)
        {
            await _db.Invoices.Where(c => c.OfferId == offerId && c.StatusId == CatalogCode_InvoiceStatus.ValidationDian)
            .ExecuteUpdateAsync(p => p
            .SetProperty(u => u.StatusId, CatalogCode_InvoiceStatus.WaitValidationDian)
            .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO()));

            return true;
        }

        public async Task<bool> ExistsStatusWaitValidationDianAsync(Guid offerId, Guid statusId)
        {
            var result = await _db.Invoices.AnyAsync(c => c.OfferId == offerId && c.StatusId == statusId);
            return result;
        }

        public List<ResumeInvoicesProcessDian> GetInvoicesProcessed(Guid offerId, int consecutive = 0)
        {
            var query = (from u in _db.Invoices
                         join o in _db.Offers on u.OfferId equals o.Id
                         from cs in _db.CatalogItems.Where(x => x.Id == u.StatusId).DefaultIfEmpty()
                         where
                         (u.OfferId == offerId || o.Consecutive == consecutive) &&
                         u.Status == true
                         select new ResumeInvoicesProcessDian
                         {
                             InvoiceNumber = u.Number.ToString(),
                             status = cs.Name.ToString(),
                         });
            return query.ToList();
        }

        public async Task<string> GetEmailUserByOffer(Guid offerId, int consecutive = 0)
        {
            var email = await (from U in _db.Users
                               join ID in _db.Offers on U.Id equals ID.UserId
                               where ID.Id == offerId || ID.Consecutive == consecutive
                               select U.Email).FirstOrDefaultAsync();
            return email;
        }
    }
}