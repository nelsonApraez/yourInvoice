///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalShareholderRepository : ILegalShareholderRepository
    {
        private readonly LinkDbContext _context;

        public LegalShareholderRepository(LinkDbContext context) {  
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public LegalShareholder AddLegalShareholder(LegalShareholder legalShareholder) => _context.LegalShareholders.Add(legalShareholder).Entity;

        public async Task<List<GetLegalShareholderResponse>> GetLegalShareholdersById(Guid id_LegalGeneralInformation)
        {
            var result = await (from T in _context.LegalShareholders
                                from D in _context.CatalogItems.Where(x => x.Id == T.DocumentTypeId).DefaultIfEmpty()
                                where T.Id_LegalGeneralInformation.Equals(id_LegalGeneralInformation)
                                orderby T.CreatedOn ascending
                                select new GetLegalShareholderResponse
                                {
                                    Id = T.Id,
                                    Id_LegalGeneralInformation = T.Id_LegalGeneralInformation,
                                    FullNameCompanyName = T.FullNameCompanyName,
                                    DocumentTypeId = T.DocumentTypeId,
                                    DocumentTypeName = D.Descripton,
                                    DocumentNumber = T.DocumentNumber,
                                    PhoneNumber = T.PhoneNumber,
                                    Completed = T.Completed,
                                    StatusId = T.StatusId,
                                    StatusDate = T.StatusDate,
                                    Status = T.Status
                                }).ToListAsync();
            return result ?? new();
        }

        public async Task<bool> UpdateLegalShareholder(LegalShareholder legalShareholder)
        {
            var result = await _context.LegalShareholders
                .Where(T => T.Id == legalShareholder.Id && T.Id_LegalGeneralInformation == legalShareholder.Id_LegalGeneralInformation)
                .ExecuteUpdateAsync(P => P
                    .SetProperty(C => C.FullNameCompanyName, legalShareholder.FullNameCompanyName)
                    .SetProperty(C => C.DocumentTypeId, legalShareholder.DocumentTypeId)
                    .SetProperty(C => C.DocumentNumber, legalShareholder.DocumentNumber)
                    .SetProperty(C => C.PhoneNumber, legalShareholder.PhoneNumber)
                    .SetProperty(C => C.Completed, legalShareholder.Completed)
                    .SetProperty(C => C.ModifiedBy, legalShareholder.Id_LegalGeneralInformation)
                    .SetProperty(C => C.ModifiedOn, ExtensionFormat.DateTimeCO())
                );
            return true;
        }

        public async Task<bool> ExistsLegalShareholderById(Guid id, Guid id_LegalGeneralInformation)
        {
            return await _context.LegalShareholders.AnyAsync(x => x.Id == id && x.Id_LegalGeneralInformation == id_LegalGeneralInformation);
        }

        public async Task<bool> DeleteLegalShareholder(Guid id, Guid id_LegalGeneralInformation)
        { 
            await _context.LegalShareholders.Where(x => x.Id == id && x.Id_LegalGeneralInformation == id_LegalGeneralInformation).ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> ExistsLegalShareholder(Guid id_LegalGeneralInformation)
        {
            return await _context.LegalShareholders.AnyAsync(x => x.Id_LegalGeneralInformation == id_LegalGeneralInformation);
        }
    }
}
