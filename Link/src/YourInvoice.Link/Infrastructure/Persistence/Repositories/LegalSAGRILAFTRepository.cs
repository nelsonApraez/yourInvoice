
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalSAGRILAFTRepository : ILegalSAGRILAFTRepository
    {
        private readonly LinkDbContext _context;

        public LegalSAGRILAFTRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateSagrilaftAsync(IEnumerable<LegalSAGRILAFT> sagrilaft)
        {
            await _context.LegalSAGRILAFT.AddRangeAsync(sagrilaft);
            return true;
        }

        public async Task<bool> ExistsSagrilaftAsync(Guid idLegalGeneralInformation)
        {
            var exist = await _context.LegalSAGRILAFT.AnyAsync(a => a.Id_LegalGeneralInformation == idLegalGeneralInformation);
            return exist;
        }

        public async Task<bool> UpdateSagrilaftAsync(LegalSAGRILAFT sagrilaft)
        {
            await _context.LegalSAGRILAFT
                  .Where(c => c.Id == sagrilaft.Id && c.Id_LegalGeneralInformation == sagrilaft.Id_LegalGeneralInformation)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.QuestionIdentifier, sagrilaft.QuestionIdentifier)
                  .SetProperty(u => u.ResponseIdentifier, sagrilaft.ResponseIdentifier)
                  .SetProperty(u => u.ResponseDetail, sagrilaft.ResponseDetail)
                  .SetProperty(u => u.Completed, sagrilaft.Completed)
                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.ModifiedBy, sagrilaft.Id_LegalGeneralInformation)
                  );

            return true;
        }

        public async Task<GetSagrilaftResponse> GetSagrilaftAsync(Guid idLegalGeneralInformation)
        {
            var result = await (from E in _context.LegalSAGRILAFT
                                from Co in _context.CatalogItems.Where(x => x.Id == E.Completed).DefaultIfEmpty()
                                where E.Id_LegalGeneralInformation == idLegalGeneralInformation && E.Status == true
                                select new GetSagrilaftResponse
                                {
                                    Completed = E.Completed,
                                    CompletedDescription = Co.Descripton,
                                    CompletedName = Co.Name,
                                    SagrilaftAnswers = (from Es in _context.LegalSAGRILAFT
                                                       from Qs in _context.CatalogItems.Where(x => x.Id == Es.QuestionIdentifier).DefaultIfEmpty()
                                                       from Rs in _context.CatalogItems.Where(x => x.Id == Es.ResponseIdentifier).DefaultIfEmpty()
                                                       where Es.Id_LegalGeneralInformation == E.Id_LegalGeneralInformation && Es.Status == true
                                                       orderby Qs.Order
                                                       select new GetSagrilaft
                                                       {
                                                           Id = Es.Id,
                                                           Id_LegaleneralInformation = Es.Id_LegalGeneralInformation,
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

        public async Task<List<GetSagrilaftQuestionsAnswerResponse>> GetSagrilaftQuestionAnswerAsync(string catalogName, List<Guid> showDetail)
        {
            var result = await (from E in _context.CatalogItems
                                where E.CatalogName == catalogName
                                orderby E.Order
                                select new GetSagrilaftQuestionsAnswerResponse
                                {
                                    IdQuestion = E.Id,
                                    DecriptionQuestion = E.Descripton,
                                    Detalle = showDetail.Contains(E.Id),
                                    Answers = _context.CatalogItems
                                                        .Where(c => c.ParentId == E.Id)
                                                        .OrderBy(c => c.Descripton)
                                                        .Select(s => new GetSagrilaftAnswer
                                                        {
                                                            IdAnswer = s.Id,
                                                            DecriptionAnswer = s.Descripton,
                                                        }).ToList()
                                }).ToListAsync();

            return result ?? new();
        }
    }
}