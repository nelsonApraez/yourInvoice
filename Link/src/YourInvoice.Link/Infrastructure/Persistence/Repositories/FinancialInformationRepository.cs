


///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class FinancialInformationRepository : IFinancialInformationRepository
    {
        private readonly LinkDbContext _context;
        private const string defaultSelection = "NO";

        public FinancialInformationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateFinancialAsync(FinancialInformation financial)
        {
            await _context.FinancialInformations.AddRangeAsync(financial);
            return true;
        }

        public async Task<bool> ExistsFinancialAsync(Guid idGeneralInformation)
        {
            var exist = await _context.FinancialInformations.AnyAsync(a => a.Id_GeneralInformation == idGeneralInformation);
            return exist;
        }


        public async Task<bool> UpdateFinancialAsync(FinancialInformation financial)
        {
            await _context.FinancialInformations
                      .Where(c => c.Id == financial.Id && c.Id_GeneralInformation == financial.Id_GeneralInformation && c.Status == true)
                      .ExecuteUpdateAsync(p => p
                      .SetProperty(u => u.TotalAssets, financial.TotalAssets)
                      .SetProperty(u => u.TotalLiabilities, financial.TotalLiabilities)
                      .SetProperty(u => u.TotalWorth, financial.TotalWorth)
                      .SetProperty(u => u.MonthlyIncome, financial.MonthlyIncome)
                      .SetProperty(u => u.MonthlyExpenditures, financial.MonthlyExpenditures)
                      .SetProperty(u => u.OtherIncome, financial.OtherIncome)
                      .SetProperty(u => u.DescribeOriginIncome, financial.DescribeOriginIncome)
                      .SetProperty(u => u.Completed, financial.Completed)
                      .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                      .SetProperty(u => u.ModifiedBy, financial.Id_GeneralInformation)
                      .SetProperty(u => u.Status, true));

            return true;
        }

        public async Task<GetFinancialResponse> GetFinancialInformationAsync(Guid idGeneralInformation)
        {
            var result = await (from E in _context.FinancialInformations
                                where E.Id_GeneralInformation == idGeneralInformation && E.Status == true
                                select new GetFinancialResponse
                                {
                                    Id = E.Id,
                                    Id_GeneralInformation = E.Id_GeneralInformation,
                                    TotalAssets = E.TotalAssets.ToString(),
                                    TotalLiabilities = E.TotalLiabilities.ToString(),
                                    TotalWorth = E.TotalWorth.ToString(),
                                    MonthlyIncome = E.MonthlyIncome.ToString(),
                                    MonthlyExpenditures=E.MonthlyExpenditures.ToString(),
                                    OtherIncome = E.OtherIncome.ToString(),
                                    DescribeOriginIncome = E.DescribeOriginIncome,
                                    Completed = E.Completed,
                                    StatusId = E.StatusId,
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }
    }
}