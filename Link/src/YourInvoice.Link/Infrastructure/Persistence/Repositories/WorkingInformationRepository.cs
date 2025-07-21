///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class WorkingInformationRepository : IWorkingInformationRepository
    {
        private readonly LinkDbContext _context;

        public WorkingInformationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateWorkingAsync(WorkingInformation workingInformation)
        {
            await _context.WorkingInformations.AddAsync(workingInformation);
            return true;
        }

        public async Task<bool> ExistsWorkingAsync(Guid idGeneralInformation)
        {
            var exist = await _context.WorkingInformations.AnyAsync(a => a.Id_GeneralInformation == idGeneralInformation);
            return exist;
        }

        public async Task<bool> UpdateWorkingAsync(WorkingInformation working)
        {
            await _context.WorkingInformations
                  .Where(c => c.Id == working.Id && c.Id_GeneralInformation == working.Id_GeneralInformation)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.BusinessName, working.BusinessName)
                  .SetProperty(u => u.Profession, working.Profession)
                  .SetProperty(u => u.Position, working.Position)
                  .SetProperty(u => u.PhoneNumber, working.PhoneNumber)
                  .SetProperty(u => u.DepartmentState, working.DepartmentState)
                  .SetProperty(u => u.City, working.City)
                  .SetProperty(u => u.Address, working.Address)
                  .SetProperty(u => u.Completed, working.Completed)
                  .SetProperty(u => u.WhatTypeProductServiceSell, working.WhatTypeProductServiceSell)
                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.ModifiedBy, working.Id_GeneralInformation));

            return true;
        }

        public async Task<GetWorkingResponse> GetWorkingAsync(Guid idGeneralInformation)
        {
            var result = await (from W in _context.WorkingInformations
                                from D in _context.CatalogItems.Where(x => x.Id == W.DepartmentState).DefaultIfEmpty()
                                from C in _context.CatalogItems.Where(x => x.Id == W.City).DefaultIfEmpty()
                                from Co in _context.CatalogItems.Where(x => x.Id == W.Completed).DefaultIfEmpty()
                                where W.Id_GeneralInformation == idGeneralInformation && W.Status == true
                                select new GetWorkingResponse
                                {
                                    Id = W.Id,
                                    Id_GeneralInformation = W.Id_GeneralInformation,
                                    BusinessName = W.BusinessName,
                                    Profession = W.Profession,
                                    Position = W.Position,
                                    PhoneNumber = W.PhoneNumber,
                                    DepartmentState = W.DepartmentState,
                                    DescriptionDepartmentState = D.Descripton,
                                    City = W.City,
                                    Address = W.Address,
                                    DescriptionCity = C.Descripton,
                                    Completed = W.Completed,
                                    DescriptionCompleted = Co.Descripton,
                                    WhatTypeProductServiceSell = W.WhatTypeProductServiceSell,
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }
    }
}