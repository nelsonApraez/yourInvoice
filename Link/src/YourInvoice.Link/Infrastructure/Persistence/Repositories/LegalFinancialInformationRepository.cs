using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalFinancialInformationRepository : ILegalFinancialInformationRepository
    {
        private readonly LinkDbContext _context;
        private const string defaultSelection = "NO";

        public LegalFinancialInformationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateLegalFinancialAsync(LegalFinancialInformation financial)
        {
            await _context.LegalFinancialInformations.AddRangeAsync(financial);
            return true;
        }

        public async Task<bool> ExistsLegalFinancialAsync(Guid idLegalGeneralInformation)
        {
            var exist = await _context.LegalFinancialInformations.AnyAsync(a => a.Id_LegalGeneralInformation == idLegalGeneralInformation);
            return exist;
        }


        public async Task<bool> UpdateLegalFinancialAsync(LegalFinancialInformation financial)
        {
            await _context.LegalFinancialInformations
                      .Where(c => c.Id == financial.Id && c.Id_LegalGeneralInformation == financial.Id_LegalGeneralInformation && c.Status == true)
                      .ExecuteUpdateAsync(p => p
                      .SetProperty(u => u.TotalAssets, financial.TotalAssets)
                      .SetProperty(u => u.TotalLiabilities, financial.TotalLiabilities)
                      .SetProperty(u => u.MonthlyIncome, financial.MonthlyIncome)
                      .SetProperty(u => u.OtherIncome, financial.OtherIncome)
                      .SetProperty(u => u.DescribeOriginIncome, financial.DescribeOriginIncome)
                      .SetProperty(u => u.Currency, financial.Currency)
                      .SetProperty(u => u.AccountNumber, financial.AccountNumber)
                      .SetProperty(u => u.AccountsForeignCurrency, financial.AccountsForeignCurrency)
                      .SetProperty(u => u.Amount, financial.Amount)
                      .SetProperty(u => u.Bank, financial.Bank)
                      .SetProperty(u => u.City, financial.City)
                      .SetProperty(u => u.OperationsForeignCurrency, financial.OperationsForeignCurrency)
                      .SetProperty(u => u.OperationsType, financial.OperationsType)
                      .SetProperty(u => u.OperationTypeDetail, financial.OperationTypeDetail)
                      .SetProperty(u => u.TotalMonthlyExpenditures, financial.TotalMonthlyExpenditures)
                      .SetProperty(u => u.TotalMonthlyIncome, financial.TotalMonthlyIncome)
                      .SetProperty(u => u.Completed, financial.Completed)
                      .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                      .SetProperty(u => u.ModifiedBy, financial.Id_LegalGeneralInformation)
                      .SetProperty(u => u.Status, true));

            return true;
        }

        public async Task<GetLegalFinancialResponse> GetLegalFinancialInformationAsync(Guid idLegalGeneralInformation)
        {
            var result = await (from E in _context.LegalFinancialInformations
                                where E.Id_LegalGeneralInformation == idLegalGeneralInformation && E.Status == true
                                select new GetLegalFinancialResponse
                                {
                                    Id = E.Id,
                                    Id_LegalGeneralInformation = E.Id_LegalGeneralInformation,
                                    TotalAssets = E.TotalAssets,
                                    TotalLiabilities = E.TotalLiabilities,
                                    MonthlyIncome = E.MonthlyIncome,
                                    OtherIncome = E.OtherIncome,
                                    DescribeOriginIncome = E.DescribeOriginIncome,
                                    AccountNumber = E.AccountNumber,
                                    AccountsForeignCurrency = E.AccountsForeignCurrency,
                                    Amount = E.Amount,
                                    Bank = E.Bank,
                                    City = E.City,
                                    Currency = E.Currency,
                                    OperationsForeignCurrency = E.OperationsForeignCurrency,
                                    OperationsTypes = ConvertStringToGuidList(E.OperationsType),
                                    OperationTypeDetail = E.OperationTypeDetail,
                                    TotalMonthlyExpenditures = E.TotalMonthlyExpenditures,
                                    TotalMonthlyIncome = E.TotalMonthlyIncome,
                                    Completed = E.Completed,
                                    StatusId = E.StatusId

                                }).FirstOrDefaultAsync();

            return result ?? new();
        }

        public static List<Guid?> ConvertStringToGuidList(string input)
        {
            List<Guid?> guidList = new List<Guid?>();

            if (!string.IsNullOrWhiteSpace(input))
            {
                string[] guids = input.Split(',');
                foreach (string guid in guids)
                {
                    if (Guid.TryParse(guid.Trim(), out Guid parsedGuid))
                    {
                        guidList.Add(parsedGuid);
                    }
                }
            }

            return guidList;
        }
    }
}
