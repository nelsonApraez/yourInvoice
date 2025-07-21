///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Users.Queries;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Domain.Roles
{
    public interface IRoleRepository
    {
        Task<List<GetRoleResponse>> GetRoleAsync(string email);
        Task<GetRoleResponse> GetRoleNewUserAsync(string email);
    }
}