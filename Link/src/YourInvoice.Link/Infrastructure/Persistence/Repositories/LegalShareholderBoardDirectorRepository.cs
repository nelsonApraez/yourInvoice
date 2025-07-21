///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalShareholderBoardDirectorRepository : ILegalShareholderBoardDirectorRepository
    {
        private readonly LinkDbContext _context;

        public LegalShareholderBoardDirectorRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public LegalShareholderBoardDirector AddLegalShareholderBoardDirector(LegalShareholderBoardDirector entity) => _context.LegalShareholdersBoardDirectors.Add(entity).Entity;

        public async Task<GetLegalShareholderBoardDirectorResponse> GetLegalShareholderBoardDirectorById(Guid id_LegalGeneralInformation)
        {
            var result = await (from T in _context.LegalShareholdersBoardDirectors
                                where T.Id_LegalGeneralInformation.Equals(id_LegalGeneralInformation)
                                select new GetLegalShareholderBoardDirectorResponse
                                {
                                    Id = T.Id,
                                    Id_LegalGeneralInformation = T.Id_LegalGeneralInformation,
                                    IsSoleProprietorship = T.IsSoleProprietorship,
                                    Completed = T.Completed,
                                    StatusId = T.StatusId,
                                    StatusDate = T.StatusDate,
                                    Status = T.Status
                                }).FirstOrDefaultAsync();
            return result ?? new();
        }

        public async Task<bool> UpdateLegalShareholderBoardDirector(LegalShareholderBoardDirector entity)
        {
            var result = await _context.LegalShareholdersBoardDirectors
                .Where(T => T.Id_LegalGeneralInformation == entity.Id_LegalGeneralInformation)
                .ExecuteUpdateAsync(P => P
                    .SetProperty(C => C.IsSoleProprietorship, entity.IsSoleProprietorship)
                    .SetProperty(C => C.Completed, entity.Completed)
                    .SetProperty(C => C.ModifiedBy, entity.Id_LegalGeneralInformation)
                    .SetProperty(C => C.ModifiedOn, ExtensionFormat.DateTimeCO())
                );
            return true;
        }

        public async Task<bool> ExistsLegalShareholderBoardDirectorById(Guid id_LegalGeneralInformation)
        {
            return await _context.LegalShareholdersBoardDirectors.AnyAsync(x => x.Id_LegalGeneralInformation == id_LegalGeneralInformation);
        }
    }
}