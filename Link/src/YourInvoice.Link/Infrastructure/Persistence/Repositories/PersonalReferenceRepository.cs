///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class PersonalReferenceRepository : IPersonalReferenceRepository
    {
        private readonly LinkDbContext _context;

        public PersonalReferenceRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GetReferenceResponse> GetPersonalReferenceAsync(Guid idGeneralInformation)
        {
            var result = await (from E in _context.PersonalReference
                                where E.Id_GeneralInformation == idGeneralInformation
                                select new GetReferenceResponse
                                {
                                    Id = E.Id,
                                    Id_GeneralInformation = idGeneralInformation,
                                    NamePersonalReference = E.NamePersonalReference,
                                    PhoneNumber = E.PhoneNumber,
                                    NameBussines = E.NameBussines,
                                    DepartmentState = E.DepartmentState,
                                    City = E.City,
                                    Completed = E.Completed,
                                    Status = E.Status,
                                    StatusDate = E.StatusDate,
                                    StatusId = E.StatusId,
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }

        public async Task<bool> ExistsPersonalReferencesByIdAsync(Guid idGeneralIfnormation) => await _context.PersonalReference.AnyAsync(reference => reference.Id_GeneralInformation == idGeneralIfnormation);

        public PersonalReferences Add(PersonalReferences personalReferences) => _context.PersonalReference.Add(personalReferences).Entity;

        public async Task<bool> UpdatePersonalReferencesAsync(PersonalReferences personalReferences)
        {
            var result = await _context.PersonalReference
                .Where(c => c.Id_GeneralInformation == personalReferences.Id_GeneralInformation)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(u => u.NamePersonalReference, personalReferences.NamePersonalReference)
                    .SetProperty(u => u.PhoneNumber, personalReferences.PhoneNumber)
                    .SetProperty(u => u.NameBussines, personalReferences.NameBussines)
                    .SetProperty(u => u.DepartmentState, personalReferences.DepartmentState)
                    .SetProperty(u => u.City, personalReferences.City)
                    .SetProperty(u => u.Completed, personalReferences.Completed)
                    .SetProperty(u => u.ModifiedBy, personalReferences.ModifiedBy)
                    .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                    .SetProperty(u => u.Status, true)
                );
            return true;
        }
    }
}
