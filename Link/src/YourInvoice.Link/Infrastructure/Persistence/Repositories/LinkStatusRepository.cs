///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LinkStatusRepository : ILinkStatusRepository
    {
        private readonly LinkDbContext _context;

        public LinkStatusRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsLinkStatusAsync(Guid idUserLink)
        {
            var exist = await _context.LinkStatus.AnyAsync(a => a.IdUserLink == idUserLink);
            return exist;
        }

        public async Task<bool> CreateLinkStatusAsync(LinkStatus linkStatus)
        {
            await _context.LinkStatus.AddAsync(linkStatus);
            return true;
        }

        public async Task<LinkStatus> GetLinkStatusAsync(Guid idUserLink)
        {
            var result = await _context.LinkStatus.FirstOrDefaultAsync(c => c.IdUserLink == idUserLink);

            return result ?? new();
        }

        public async Task<GetLinkStatusEnableResponse> GetLinkStatusDisabledAsync(Guid idUserLink)
        {
            var result = await (from L in _context.LinkStatus
                                from Co in _context.CatalogItems.Where(x => x.Id == L.StatusLinkId).DefaultIfEmpty()
                                where L.IdUserLink == idUserLink
                                select new GetLinkStatusEnableResponse
                                {
                                    LinkStatusId = L.StatusLinkId,
                                    LinkStatusDescription = Co.Descripton
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }

        public async Task<bool> UpdateLinkStatusAsync(LinkStatus linkStatus)
        {
            await _context.LinkStatus
                  .Where(c => c.IdUserLink == linkStatus.IdUserLink)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.StatusLinkId, linkStatus.StatusLinkId)
                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.ModifiedBy, linkStatus.IdUserLink));
            return true;
        }

    }
}