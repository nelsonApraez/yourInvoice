

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalCommercialAndBankReferenceRepository : ILegalCommercialAndBankReferenceRepository
    {
        private readonly LinkDbContext _context;
        private const string defaultSelection = "NO";

        public LegalCommercialAndBankReferenceRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateLegalCommercialAndBankReferenceAsync(LegalCommercialAndBankReference reference)
        {
            await _context.LegalCommercialAndBankReferences.AddRangeAsync(reference);
            return true;
        }

        public async Task<bool> ExistsLegalCommercialAndBankReferenceAsync(Guid idLegalGeneralInformation)
        {
            var exist = await _context.LegalCommercialAndBankReferences.AnyAsync(a => a.Id_LegalGeneralInformation == idLegalGeneralInformation);
            return exist;
        }


        public async Task<bool> UpdateLegalCommercialAndBankReferenceAsync(LegalCommercialAndBankReference reference)
        {
            await _context.LegalCommercialAndBankReferences
                      .Where(c => c.Id == reference.Id && c.Id_LegalGeneralInformation == reference.Id_LegalGeneralInformation && c.Status == true)
                      .ExecuteUpdateAsync(p => p
                      .SetProperty(u => u.BankReference, reference.BankReference)
                      .SetProperty(u => u.CommercialReference, reference.CommercialReference)
                      .SetProperty(u => u.CityCommercial, reference.CityCommercial)
                      .SetProperty(u => u.CityBank, reference.CityBank)
                      .SetProperty(u => u.DepartmentStateBank, reference.DepartmentStateBank)
                      .SetProperty(u => u.DepartmentStateCommercial, reference.DepartmentStateCommercial)
                      .SetProperty(u => u.PhoneNumberBank, reference.PhoneNumberBank)
                      .SetProperty(u => u.PhoneNumberCommercial, reference.PhoneNumberCommercial)
                      .SetProperty(u => u.Completed, reference.Completed)
                      .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                      .SetProperty(u => u.ModifiedBy, reference.Id_LegalGeneralInformation)
                      .SetProperty(u => u.Status, true));

            return true;
        }

        public async Task<LegalCommercialAndBankReferenceResponse> GetLegalCommercialAndBankReferenceAsync(Guid idLegalGeneralInformation)
        {
            var result = await (from E in _context.LegalCommercialAndBankReferences
                                where E.Id_LegalGeneralInformation == idLegalGeneralInformation && E.Status == true
                                select new LegalCommercialAndBankReferenceResponse
                                {
                                    Id = E.Id,
                                    PhoneNumberBank = E.PhoneNumberBank,
                                    BankReference = E.BankReference,
                                    PhoneNumberCommercial = E.PhoneNumberCommercial,
                                    CommercialReference = E.CommercialReference,
                                    DepartmentStateCommercial = E.DepartmentStateCommercial,
                                    CityBank = E.CityBank,
                                    CityCommercial = E.CityCommercial,
                                    DepartmentStateBank = E.DepartmentStateBank,
                                    Id_LegalGeneralInformation = E.Id_LegalGeneralInformation,
                                    Completed = E.Completed,
                                    StatusId = E.StatusId,
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }
    }
}
