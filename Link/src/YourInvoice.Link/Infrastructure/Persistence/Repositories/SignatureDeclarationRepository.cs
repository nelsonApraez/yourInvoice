using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class SignatureDeclarationRepository : ISignatureDeclarationRepository
    {

        private readonly LinkDbContext _context;

        public SignatureDeclarationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateSignatureDeclarationAsync(SignatureDeclaration signatureDeclaration)
        {
            await _context.SignatureDeclaration.AddRangeAsync(signatureDeclaration);
            return true;
        }

        public async Task<bool> UpdateSignatureDeclarationAsync(SignatureDeclaration signatureDeclaration)
        {
            await _context.SignatureDeclaration
                  .Where(c => c.Id_GeneralInformation == signatureDeclaration.Id_GeneralInformation)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.SourceFundsDeclaration, signatureDeclaration.SourceFundsDeclaration)
                  .SetProperty(u => u.VisitAuthorization, signatureDeclaration.VisitAuthorization)
                  .SetProperty(u => u.GeneralStatement, signatureDeclaration.GeneralStatement)
                  .SetProperty(u => u.StatusId, signatureDeclaration.StatusId)
                  .SetProperty(u => u.Completed, signatureDeclaration.Completed)
                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.ModifiedBy, signatureDeclaration.ModifiedBy));

            return true;
        }

        public async Task<GetSignatureDeclarationResponse> GetSignatureDeclarationAsync(Guid idGeneralInformation)
        {
            var result = await (from E in _context.SignatureDeclaration
                                where E.Id_GeneralInformation == idGeneralInformation && E.Status == true
                                select new GetSignatureDeclarationResponse
                                {
                                    Id = E.Id,
                                    Id_GeneralInformation = idGeneralInformation,
                                    SourceFundsDeclaration = E.SourceFundsDeclaration,
                                    VisitAuthorization = E.VisitAuthorization,
                                    GeneralStatement = E.GeneralStatement,
                                    Completed = E.Completed,
                                    StatusId = E.StatusId

                                }).FirstOrDefaultAsync();

            return result ?? new();
        }

        public async Task<bool> ExistsSignatureDeclarationByIdAsync(Guid idGeneralIfnormation) => await _context.SignatureDeclaration.AnyAsync(SignatureDeclaration => SignatureDeclaration.Id_GeneralInformation == idGeneralIfnormation);

    }
}
