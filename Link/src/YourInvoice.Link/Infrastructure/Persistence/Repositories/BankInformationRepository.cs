

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class BankInformationRepository : IBankInformationRepository
    {
        private readonly LinkDbContext _context;
        private const string defaultSelection = "NO";

        public BankInformationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateBankAsync(BankInformation bank)
        {
            await _context.BankInformations.AddRangeAsync(bank);
            return true;
        }

        public async Task<bool> ExistsBankAsync(Guid idGeneralInformation)
        {
            var exist = await _context.BankInformations.AnyAsync(a => a.Id_GeneralInformation == idGeneralInformation);
            return exist;
        }

        public async Task<bool> UpdateBankAsync(BankInformation bank)
        {
            await _context.BankInformations
                      .Where(c => c.Id == bank.Id && c.Id_GeneralInformation == bank.Id_GeneralInformation && c.Status == true)
                      .ExecuteUpdateAsync(p => p
                      .SetProperty(u => u.BankReference, bank.BankReference)
                      .SetProperty(u => u.PhoneNumber, bank.PhoneNumber)
                      .SetProperty(u => u.BankProduct, bank.BankProduct)
                      .SetProperty(u => u.DepartmentState, bank.DepartmentState)
                      .SetProperty(u => u.City, bank.City)
                      .SetProperty(u => u.Completed, bank.Completed)
                      .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                      .SetProperty(u => u.ModifiedBy, bank.Id_GeneralInformation)
                      .SetProperty(u => u.Status, true));

            return true;
        }

        public async Task<GetBankResponse> GetbankInformationAsync(Guid idGeneralInformation)
        {
            var result = await (from E in _context.BankInformations
                                where E.Id_GeneralInformation == idGeneralInformation && E.Status == true
                                select new GetBankResponse
                                {
                                    Id = E.Id,
                                    Id_GeneralInformation = E.Id_GeneralInformation,
                                    BankReference = E.BankReference,
                                    PhoneNumber = E.PhoneNumber,
                                    BankProduct = E.BankProduct,
                                    DepartmentState=E.DepartmentState,
                                    City = E.City,
                                    Completed = E.Completed,
                                    StatusId = E.StatusId,
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }
    }
}