///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations
{
    public interface IGeneralInformationRepository
    {
        Task<IEnumerable<GeneralInformation>> GetAllGeneralInformationAsync();

        GeneralInformation Add(GeneralInformation generalInformation);

        Task<bool> UpdateStatusAsync(Guid id, Guid statusId);

        Task<GeneralInformationResponse> GetGeneralInformationIdAsync(Guid Id);

        Task<bool> UpdateAsync(GeneralInformation generalInformation);

        Task<GeneralInformation?> GetByEmailAsync(string email);

        Task<bool> UpdateGeneralInformationAsync(GeneralInformation generalInformation);

    }
}
