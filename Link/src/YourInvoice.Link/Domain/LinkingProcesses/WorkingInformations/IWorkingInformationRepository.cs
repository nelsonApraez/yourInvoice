
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations
{
    public interface IWorkingInformationRepository
    {
        Task<bool> CreateWorkingAsync(WorkingInformation workingInformation);

        Task<bool> ExistsWorkingAsync(Guid idGeneralInformation);
        Task<GetWorkingResponse> GetWorkingAsync(Guid idGeneralInformation);
        Task<bool> UpdateWorkingAsync(WorkingInformation working);
    }
}