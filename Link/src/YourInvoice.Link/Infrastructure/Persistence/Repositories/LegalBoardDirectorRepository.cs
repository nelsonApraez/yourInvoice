///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalBoardDirectorRepository : ILegalBoardDirectorRepository
    {
        private readonly LinkDbContext _context;

        public LegalBoardDirectorRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public LegalBoardDirector AddLegalBoardDirector(LegalBoardDirector entity) => _context.LegalBoardDirectors.Add(entity).Entity;

        public async Task<List<GetLegalBoardDirectorResponse>> GetLegalBoardDirectorById(Guid id_LegalGeneralInformation)
        {
            var result = await (from T in _context.LegalBoardDirectors
                                from D in _context.CatalogItems.Where(x => x.Id == T.DocumentTypeId).DefaultIfEmpty()
                                where T.Id_LegalGeneralInformation.Equals(id_LegalGeneralInformation)
                                orderby T.CreatedOn ascending
                                select new GetLegalBoardDirectorResponse
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

        public async Task<bool> UpdatelegalBoardDirector(LegalBoardDirector legalBoardDirector)
        {
            var result = await _context.LegalBoardDirectors
                .Where(T => T.Id == legalBoardDirector.Id && T.Id_LegalGeneralInformation == legalBoardDirector.Id_LegalGeneralInformation)
                .ExecuteUpdateAsync(P => P
                    .SetProperty(C => C.FullNameCompanyName, legalBoardDirector.FullNameCompanyName)
                    .SetProperty(C => C.DocumentTypeId, legalBoardDirector.DocumentTypeId)
                    .SetProperty(C => C.DocumentNumber, legalBoardDirector.DocumentNumber)
                    .SetProperty(C => C.PhoneNumber, legalBoardDirector.PhoneNumber)
                    .SetProperty(C => C.Completed, legalBoardDirector.Completed)
                    .SetProperty(C => C.ModifiedBy, legalBoardDirector.Id_LegalGeneralInformation)
                    .SetProperty(C => C.ModifiedOn, ExtensionFormat.DateTimeCO())
                );
            return true;
        }

        public async Task<bool> ExistsLegalBoardDirectorById(Guid id, Guid id_LegalGeneralInformation)
        {
            return await _context.LegalBoardDirectors.AnyAsync(x => x.Id == id && x.Id_LegalGeneralInformation == id_LegalGeneralInformation);
        }

        public async Task<bool> DeleteLegalBoardDirector(Guid id, Guid id_LegalGeneralInformation)
        {
            await _context.LegalBoardDirectors.Where(x => x.Id == id && x.Id_LegalGeneralInformation == id_LegalGeneralInformation).ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> ExistsLegalBoardDirector(Guid id_LegalGeneralInformation)
        {
            return await _context.LegalBoardDirectors.AnyAsync(x => x.Id_LegalGeneralInformation == id_LegalGeneralInformation);
        }
    }
}
