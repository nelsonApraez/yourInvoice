///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.EF.Entity;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Common.EF.Data.IRepositories
{
    public interface IUserRepository : IRepository<UserInfo>
    {
        Task<List<GetRoleResponse>> GetRoleAsync(string email);
    }
}