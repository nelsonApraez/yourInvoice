///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class GeneralInformationRepository : IGeneralInformationRepository
    {
        private readonly LinkDbContext _context;

        public GeneralInformationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public GeneralInformation Add(GeneralInformation generalInformation) => _context.GeneralInformations.Add(generalInformation).Entity;

        public async Task<bool> UpdateAsync(GeneralInformation generalInformation)
        {
            if (generalInformation != null)
            {
                _context.GeneralInformations.Update(generalInformation);

                return true;
            }
            else
            {
                return false; // Cuenta con el ID especificado no encontrada
            }
        }

        public async Task<bool> UpdateStatusAsync(Guid id, Guid statusId)
        {
            await _context.GeneralInformations
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(p => p
                .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                .SetProperty(u => u.ModifiedBy, id)
                .SetProperty(u => u.StatusId, statusId));

            return true;
        }

        public async Task<GeneralInformation?> GetByEmailAsync(string email) => await _context.GeneralInformations.SingleOrDefaultAsync(c => c.Email == email);

        public async Task<IEnumerable<GeneralInformation>> GetAllGeneralInformationAsync()
        {
            var result = await _context.GeneralInformations.ToListAsync();

            return result;
        }

        public async Task<GeneralInformationResponse> GetGeneralInformationIdAsync(Guid Id)
        {
            var result = (from A in _context.GeneralInformations
                          where A.Id == Id
                          select new GeneralInformationResponse
                          {
                              Id = A.Id,
                              FirstName = A.FirstName,
                              SecondName = A.SecondName,
                              LastName = A.LastName,
                              SecondLastName = A.SecondLastName,
                              DocumentTypeId = A.DocumentTypeId,
                              DocumentNumber = A.DocumentNumber,
                              ExpeditionDate = A.ExpeditionDate,
                              ExpeditionCountry = A.ExpeditionCountry,
                              EconomicActivity = A.EconomicActivity,
                              SecondaryEconomicActivity = A.SecondaryEconomicActivity,
                              Email = A.Email,
                              MovilPhoneNumber = A.MovilPhoneNumber,
                              PhoneNumber = A.PhoneNumber,
                              DepartmentState = A.DepartmentState,
                              City = A.City,
                              Address = A.Address,
                              PhoneCorrespondence = A.PhoneCorrespondence,
                              DepartmentStateCorrespondence = A.DepartmentStateCorrespondence,
                              CityCorrespondence = A.CityCorrespondence,
                              AddressCorrespondence = A.AddressCorrespondence,
                              Completed = A.Completed,
                              StatusId = A.StatusId,
                              StatusDate = A.StatusDate
                          });

            return await result.SingleOrDefaultAsync();


        }

        public async Task<bool> UpdateGeneralInformationAsync(GeneralInformation generalInformation)
        {
            await _context.GeneralInformations
                      .Where(c => c.Id == generalInformation.Id && c.Status == true)
                      .ExecuteUpdateAsync(p => p
                      .SetProperty(u => u.FirstName, generalInformation.FirstName)
                      .SetProperty(u => u.SecondName, generalInformation.SecondName)
                      .SetProperty(u => u.LastName, generalInformation.LastName)
                      .SetProperty(u => u.SecondLastName, generalInformation.SecondLastName)
                      .SetProperty(u => u.DocumentTypeId, generalInformation.DocumentTypeId)
                      .SetProperty(u => u.DocumentNumber, generalInformation.DocumentNumber)
                      .SetProperty(u => u.ExpeditionDate, generalInformation.ExpeditionDate)
                      .SetProperty(u => u.ExpeditionCountry, generalInformation.ExpeditionCountry)
                      .SetProperty(u => u.EconomicActivity, generalInformation.EconomicActivity)
                      .SetProperty(u => u.SecondaryEconomicActivity, generalInformation.SecondaryEconomicActivity)
                      .SetProperty(u => u.PhoneNumber, generalInformation.PhoneNumber)
                      .SetProperty(u => u.MovilPhoneNumber, generalInformation.MovilPhoneNumber)
                      .SetProperty(u => u.DepartmentState, generalInformation.DepartmentState)
                      .SetProperty(u => u.City, generalInformation.City)
                      .SetProperty(u => u.Address, generalInformation.Address)
                      .SetProperty(u => u.PhoneCorrespondence, generalInformation.PhoneCorrespondence)
                      .SetProperty(u => u.DepartmentStateCorrespondence, generalInformation.DepartmentStateCorrespondence)
                      .SetProperty(u => u.CityCorrespondence, generalInformation.CityCorrespondence)
                      .SetProperty(u => u.AddressCorrespondence, generalInformation.AddressCorrespondence)
                      .SetProperty(u => u.StatusId, generalInformation.StatusId)
                      .SetProperty(u => u.StatusDate, ExtensionFormat.DateTimeCO())
                      .SetProperty(u => u.Completed, generalInformation.Completed)
                      .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                      .SetProperty(u => u.ModifiedBy, generalInformation.Id)
                      .SetProperty(u => u.Status, true));

            await _context.Accounts
                .Where(x => x.Id == generalInformation.Id)
                .ExecuteUpdateAsync(p => p
                .SetProperty(u => u.Name, generalInformation.FirstName)
                .SetProperty(u => u.SecondName, generalInformation.SecondName)
                .SetProperty(u => u.LastName, generalInformation.LastName)
                .SetProperty(u => u.SecondLastName, generalInformation.SecondLastName)
                );

            return true;
        }

    }
}
