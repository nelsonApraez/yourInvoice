
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Domain.LinkingProcesses.LinkStatus
{
    public interface ILinkStatusRepository
    {
        Task<bool> CreateLinkStatusAsync(LinkStatus linkStatus);

        Task<bool> ExistsLinkStatusAsync(Guid idUserLink);
        Task<LinkStatus> GetLinkStatusAsync(Guid idUserLink);
        Task<GetLinkStatusEnableResponse> GetLinkStatusDisabledAsync(Guid idUserLink);
        Task<bool> UpdateLinkStatusAsync(LinkStatus linkStatus);

    }
}