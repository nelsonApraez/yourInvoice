///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalRepresentativeTaxAuditorRepository : ILegalRepresentativeTaxAuditorRepository
    {
        private readonly LinkDbContext _context;

        public LegalRepresentativeTaxAuditorRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistseLegalRepresentativeTaxAuditorRepositoryAsync(Guid id_legalRepresentativeTax)
        {
            var exist = await _context.LegalRepresentativeTaxAuditors.AnyAsync(a => a.Id_LegalGeneralInformation == id_legalRepresentativeTax);
            return exist;
        }

        public async Task<bool> CreateLegalRepresentativeTaxAuditorRepositoryAsync(LegalRepresentativeTaxAuditor legalRepresentativeTax)
        {
            await _context.LegalRepresentativeTaxAuditors.AddAsync(legalRepresentativeTax);
            return true;
        }

        public async Task<bool> UpdateLegalRepresentativeTaxAuditorAsync(LegalRepresentativeTaxAuditor legalRepresentativeTax)
        {
            await _context.LegalRepresentativeTaxAuditors
                  .Where(c => c.Id_LegalGeneralInformation == legalRepresentativeTax.Id_LegalGeneralInformation)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.FirstName, legalRepresentativeTax.FirstName)
                  .SetProperty(u => u.SecondName, legalRepresentativeTax.SecondName)
                  .SetProperty(u => u.LastName, legalRepresentativeTax.LastName)
                  .SetProperty(u => u.SecondLastName, legalRepresentativeTax.SecondLastName)
                  .SetProperty(u => u.DocumentTypeId, legalRepresentativeTax.DocumentTypeId)
                  .SetProperty(u => u.DocumentNumber, legalRepresentativeTax.DocumentNumber)
                  .SetProperty(u => u.ExpeditionDate, legalRepresentativeTax.ExpeditionDate)
                  .SetProperty(u => u.ExpeditionCountry, legalRepresentativeTax.ExpeditionCountry)
                  .SetProperty(u => u.HomeAddress, legalRepresentativeTax.HomeAddress)
                  .SetProperty(u => u.Email, legalRepresentativeTax.Email)
                  .SetProperty(u => u.Phone, legalRepresentativeTax.Phone)
                  .SetProperty(u => u.DepartmentState, legalRepresentativeTax.DepartmentState)
                  .SetProperty(u => u.City, legalRepresentativeTax.City)
                  .SetProperty(u => u.TaxAuditorFirstName, legalRepresentativeTax.TaxAuditorFirstName)
                  .SetProperty(u => u.TaxAuditorSecondName, legalRepresentativeTax.TaxAuditorSecondName)
                  .SetProperty(u => u.TaxAuditorLastName, legalRepresentativeTax.TaxAuditorLastName)
                  .SetProperty(u => u.TaxAuditorSecondLastName, legalRepresentativeTax.TaxAuditorSecondLastName)
                  .SetProperty(u => u.TaxAuditorDocumentTypeId, legalRepresentativeTax.TaxAuditorDocumentTypeId)
                  .SetProperty(u => u.TaxAuditorDocumentNumber, legalRepresentativeTax.TaxAuditorDocumentNumber)
                  .SetProperty(u => u.TaxAuditorPhoneNumber, legalRepresentativeTax.TaxAuditorPhoneNumber)
                  .SetProperty(u => u.StatusDate, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.StatusId, legalRepresentativeTax.Completed)
                  .SetProperty(u => u.Completed, legalRepresentativeTax.Completed)
                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.ModifiedBy, legalRepresentativeTax.Id));
            return true;
        }

        public async Task<LegalRepresentativeTaxAuditor> GetLegalRepresentativeTaxAuditorAsync(Guid id_LegalGeneralInformation)
        {
            var result = await _context.LegalRepresentativeTaxAuditors.Where(c => c.Id_LegalGeneralInformation == id_LegalGeneralInformation).FirstOrDefaultAsync();

            return result ?? new();
        }

        public async Task<bool> UpdateAccountAsync(LegalRepresentativeTaxAuditor legalRepresentativeTax)
        {
            await _context.Accounts
                  .Where(c => c.Id == legalRepresentativeTax.Id_LegalGeneralInformation)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.Name, legalRepresentativeTax.FirstName)
                  .SetProperty(u => u.SecondName, legalRepresentativeTax.SecondName)
                  .SetProperty(u => u.LastName, legalRepresentativeTax.LastName)
                  .SetProperty(u => u.SecondLastName, legalRepresentativeTax.SecondLastName));
            return true;
        }
    }
}