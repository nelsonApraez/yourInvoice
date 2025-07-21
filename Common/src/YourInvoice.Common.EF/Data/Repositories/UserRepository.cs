///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Common.EF.Data.Repositories
{
    public class UserRepository : Repository<UserInfo>, IUserRepository
    {
        private yourInvoiceCommonDbContext _db;

        public UserRepository(yourInvoiceCommonDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public async Task<List<GetRoleResponse>> GetRoleAsync(string email)
        {
            var result = await (from u in _db.Users
                                join r in _db.CatalogItems on u.RoleId equals r.Id
                                where u.Email == email
                                select new GetRoleResponse
                                {
                                    RoleId = r.Id,
                                    RoleName = r.Name,
                                }).ToListAsync();
            return result;
        }
    }
}