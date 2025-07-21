///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Person;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using System.Linq.Expressions;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly LinkDbContext _context;

        public PersonRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ExposureInformation>> GetExposureInformationAsync(Guid IdGeneralInformation)
        {
            var result = await _context.ExposureInformations.Where(c => c.Id_GeneralInformation == IdGeneralInformation).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CatalogItemInfo>> GetCatalogItemsAsync(string catalogName)
        {
            var result = await _context.CatalogItems.Where(c => c.CatalogName == catalogName).ToListAsync();

            return result ?? new();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            T item = null;
            item = await _context.Set<T>().FirstOrDefaultAsync(predicate);
            return item;
        }

        public async Task<GetDataLinkResponse> GetDataLinkNaturalUserAsync(Guid idUserLink)
        {
            var result = await (from G in _context.GeneralInformations
                                from C in _context.CatalogItems.Where(x => x.Id == G.City).DefaultIfEmpty()
                                from DE in _context.CatalogItems.Where(x => x.Id == G.ExpeditionCountry).DefaultIfEmpty()
                                from W in _context.WorkingInformations.Where(x => x.Id_GeneralInformation == G.Id).DefaultIfEmpty()
                                where G.Id == idUserLink
                                select new GetDataLinkResponse
                                {
                                    Address = G.Address,
                                    City = C.Descripton,
                                    Job = W.Position ?? W.Profession,
                                    DocumentExpedition = DE.Descripton,
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }

        public async Task<GetDataLinkResponse> GetDataLinkLegalUserAsync(Guid idUserLink)
        {
            var ExpeditionCountry = await (from G in _context.LegalRepresentativeTaxAuditors
                                where G.Id_LegalGeneralInformation == idUserLink
                                select Guid.Parse(G.ExpeditionCountry)).FirstOrDefaultAsync();

            var result = await (from G in _context.LegalGeneralInformations
                                from C in _context.CatalogItems.Where(x => x.Id == G.CityId).DefaultIfEmpty()
                                from DE in _context.CatalogItems.Where(x => x.Id == ExpeditionCountry).DefaultIfEmpty()
                                where G.Id == idUserLink
                                select new GetDataLinkResponse
                                {
                                    Address = G.Address,
                                    City = C.Descripton,
                                    Job = G.BranchPosition,
                                    DocumentExpedition = DE.Descripton,
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }
    }
}