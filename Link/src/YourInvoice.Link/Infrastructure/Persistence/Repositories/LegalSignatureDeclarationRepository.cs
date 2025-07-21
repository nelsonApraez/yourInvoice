///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalSignatureDeclarationRepository : ILegalSignatureDeclarationRepository
    {
        private readonly LinkDbContext _context;

        public LegalSignatureDeclarationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<string>> GeParagraphAsync(string catalogName)
        {
            var result = await _context.CatalogItems.Where(c => c.CatalogName == catalogName).OrderBy(o => o.Order).Select(c => c.Descripton).ToListAsync();

            return result;
        }

        public async Task<GetAccounLegalGeneralResponse> GetAccounLegalGeneralAsync(Guid idLegalGeneralInformation)
        {
            var result = await (from C in _context.Accounts
                                from D in _context.CatalogItems.Where(x => x.Id == C.DocumentTypeId).DefaultIfEmpty()
                                from LG in _context.LegalGeneralInformations.Where(x => x.Id == C.Id).DefaultIfEmpty()
                                from CI in _context.CatalogItems.Where(x => x.Id == LG.CityId).DefaultIfEmpty()
                                where C.Id == idLegalGeneralInformation
                                select new GetAccounLegalGeneralResponse
                                {
                                    Name = C.Name,
                                    SecondName = C.SecondName,
                                    LastName = C.LastName,
                                    SecondLastName = C.SecondLastName,
                                    CheckDigit = string.IsNullOrEmpty(LG.CheckDigit) ? C.DigitVerify : LG.CheckDigit,
                                    DocumentNumber = C.DocumentNumber,
                                    Nit = string.IsNullOrEmpty(LG.Nit) ? C.Nit : LG.Nit,
                                    SocialReason = string.IsNullOrEmpty(LG.CompanyName) ? C.SocialReason : LG.CompanyName,
                                    DocumentTypeDescription = D.Descripton,
                                    City = CI.Descripton
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }

        public async Task<LegalSignatureDeclaration> GetLegalSignatureDeclarationAsync(Guid idLegalGeneralInformation)
        {
            var result = await (from LS in _context.LegalSignatureDeclarations
                                where LS.Id_LegalGeneralInformation == idLegalGeneralInformation
                                select LS).FirstOrDefaultAsync();

            return result ?? new();
        }

        public async Task<bool> CreateLegalSignatureDeclarationAsync(LegalSignatureDeclaration legalSignatureDeclaration)
        {
            await _context.LegalSignatureDeclarations.AddRangeAsync(legalSignatureDeclaration);
            return true;
        }

        public async Task<bool> ExistsLegalSignatureDeclarationAsync(Guid idLegalGeneralInformation)
        {
            var exist = await _context.LegalSignatureDeclarations.AnyAsync(a => a.Id_LegalGeneralInformation == idLegalGeneralInformation);
            return exist;
        }

        public async Task<bool> UpdateLegalSignatureDeclarationAsync(LegalSignatureDeclaration legalSignatureDeclaration)
        {
            await _context.LegalSignatureDeclarations
                  .Where(c => c.Id_LegalGeneralInformation == legalSignatureDeclaration.Id_LegalGeneralInformation)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.CommitmentAcceptRiskManagement, legalSignatureDeclaration.CommitmentAcceptRiskManagement)
                  .SetProperty(u => u.ResponsivilityForInformation, legalSignatureDeclaration.ResponsivilityForInformation)
                  .SetProperty(u => u.Statements, legalSignatureDeclaration.Statements)
                  .SetProperty(u => u.VisitAuthorization, legalSignatureDeclaration.VisitAuthorization)
                  .SetProperty(u => u.StatusDate, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.Completed, legalSignatureDeclaration.Completed)
                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.ModifiedBy, legalSignatureDeclaration.Id_LegalGeneralInformation));

            return true;
        }
    }
}