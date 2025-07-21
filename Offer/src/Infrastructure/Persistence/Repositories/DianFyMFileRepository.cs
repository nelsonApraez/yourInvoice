///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.DianFyMFiles;
using yourInvoice.Offer.Domain.DianFyMFiles.Queries;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class DianFyMFileRepository : IDianFyMFileRepository
    {
        private readonly ApplicationDbContext _context;

        public DianFyMFileRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddAsync(List<DianFyMFile> dianFyMFiles)
        {
            await _context.DianFyMFiles.AddRangeAsync(dianFyMFiles);
            return true;
        }

        public async Task<bool> UpdateStartDateAsync(IEnumerable<DianFyMFile> dianFyMFileId)
        {
            await _context.DianFyMFiles.Where(c => dianFyMFileId.Select(s => s.Id.ToString()).ToList().Contains(c.Id.ToString()))
                                        .ExecuteUpdateAsync(p => p
                                        .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO()));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStateToProcessAsync(IEnumerable<DianFyMFile> dianFyMFileId)
        {
            await _context.DianFyMFiles.Where(c => dianFyMFileId.Select(s => s.Id.ToString()).ToList().Contains(c.Id.ToString()))
                                        .ExecuteUpdateAsync(p => p
                                        .SetProperty(u => u.Status, false)
                                        .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO()));

            return true;
        }

        public async Task<IEnumerable<string>> GetOfferAwitEndProcessDianAsync(Guid statusId)
        {
            var result = await (from I in _context.Invoices
                                join O in _context.Offers on I.OfferId equals O.Id
                                where I.StatusId == statusId
                                select $"_{O.Consecutive}_").Distinct().ToListAsync();

            return result;
        }

        public async Task<IEnumerable<DianFyMFile>> GetDianNemeFilesNoProcessAsync()
        {
            var result = await _context.DianFyMFiles.Where(c => c.Status == true).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<InvoiceCufeDian>> GetInvoiceAsync(int offer, Guid statusId)
        {
            var result = await (from I in _context.Invoices
                                join O in _context.Offers on I.OfferId equals O.Id
                                where O.Consecutive == offer && I.StatusId == statusId
                                select new InvoiceCufeDian
                                {
                                    CUFE = I.Cufe,
                                    EnvoiceId = I.Id,
                                }).ToListAsync();

            return result;
        }

        public async Task<bool> UpdateCountRegisterAsync(Guid id, int countRegister)
        {
            await _context.DianFyMFiles.Where(c => c.Id == id)
                                       .ExecuteUpdateAsync(p => p
                                       .SetProperty(c => c.CountRegisterFile, countRegister)
                                       .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO()));
            return true;
        }
    }
}