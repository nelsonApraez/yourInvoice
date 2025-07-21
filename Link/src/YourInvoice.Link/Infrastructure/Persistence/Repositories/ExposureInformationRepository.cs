///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class ExposureInformationRepository : IExposureInformationRepository
    {
        private readonly LinkDbContext _context;

        public ExposureInformationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateExposureAsync(IEnumerable<ExposureInformation> exposure)
        {
            await _context.ExposureInformations.AddRangeAsync(exposure);
            return true;
        }

        public async Task<bool> ExistsExposureAsync(Guid idGeneralInformation)
        {
            var exist = await _context.ExposureInformations.AnyAsync(a => a.Id_GeneralInformation == idGeneralInformation);
            return exist;
        }

        public async Task<bool> UpdateExposureAsync(ExposureInformation exposure)
        {
            await _context.ExposureInformations
                  .Where(c => c.Id == exposure.Id && c.Id_GeneralInformation == exposure.Id_GeneralInformation)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.QuestionIdentifier, exposure.QuestionIdentifier)
                  .SetProperty(u => u.ResponseIdentifier, exposure.ResponseIdentifier)
                  .SetProperty(u => u.ResponseDetail, exposure.ResponseDetail)
                  .SetProperty(u => u.Completed, exposure.Completed)
                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.ModifiedBy, exposure.Id_GeneralInformation)
                  .SetProperty(u => u.DeclarationOriginFunds, exposure.DeclarationOriginFunds));

            return true;
        }

        public async Task<GetExposureResponse> GetExposureAsync(Guid idGeneralInformation)
        {
            var result = await (from E in _context.ExposureInformations
                                from Co in _context.CatalogItems.Where(x => x.Id == E.Completed).DefaultIfEmpty()
                                where E.Id_GeneralInformation == idGeneralInformation && E.Status == true
                                select new GetExposureResponse
                                {
                                    Completed = E.Completed,
                                    CompletedDescription = Co.Descripton,
                                    CompletedName = Co.Name,
                                    DeclarationOriginFunds = E.DeclarationOriginFunds,
                                    ExposureAnswers = (from Es in _context.ExposureInformations
                                                       from Qs in _context.CatalogItems.Where(x => x.Id == Es.QuestionIdentifier).DefaultIfEmpty()
                                                       from Rs in _context.CatalogItems.Where(x => x.Id == Es.ResponseIdentifier).DefaultIfEmpty()
                                                       where Es.Id_GeneralInformation == E.Id_GeneralInformation && Es.Status == true
                                                       orderby Qs.Order
                                                       select new GetExposure
                                                       {
                                                           Id = Es.Id,
                                                           Id_GeneralInformation = Es.Id_GeneralInformation,
                                                           QuestionIdentifier = Es.QuestionIdentifier,
                                                           QuestionIdentifierDescription = Qs.Descripton,
                                                           ResponseIdentifier = Es.ResponseIdentifier,
                                                           ResponseIdentifierDescription = Rs.Descripton,
                                                           ResponseDetail = Es.ResponseDetail,
                                                       }).ToList(),
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }

        public async Task<ListDataInfo<ListEconomicActivityResponse>?> GetEconomicActitiesListAsync()
        {
            var query = from A in _context.EconomicActivities
                        where A.Status == true
                        orderby A.Code
                        select new ListEconomicActivityResponse
                        {
                            Id = A.Id,
                            Code = A.Code,
                            Description = A.Code + " " + A.Description
                        };
            var result = new ListDataInfo<ListEconomicActivityResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.ToListAsync()
            };
            return result;
        }

        public async Task<List<GetExposureQuestionsAnswerResponse>> GetExposureQuestionAnswerAsync(string catalogName, List<Guid> showDetail)
        {
            var result = await (from E in _context.CatalogItems
                                where E.CatalogName == catalogName
                                orderby E.Order
                                select new GetExposureQuestionsAnswerResponse
                                {
                                    IdQuestion = E.Id,
                                    DecriptionQuestion = E.Descripton,
                                    Detalle = showDetail.Contains(E.Id),
                                    Answers = _context.CatalogItems
                                                        .Where(c => c.ParentId == E.Id)
                                                        .OrderBy(c => c.Descripton)
                                                        .Select(s => new GetExposureAnswer
                                                        {
                                                            IdAnswer = s.Id,
                                                            DecriptionAnswer = s.Descripton,
                                                        }).ToList()
                                }).ToListAsync();

            return result ?? new();
        }
    }
}