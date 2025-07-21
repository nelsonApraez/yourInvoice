///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Link.Domain.Users
{
    public interface IUserLinkRepository
    {
        Task<bool> AddAsync(UserInfo user);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> ExistsUserAsync(Guid idUser);
    }
}