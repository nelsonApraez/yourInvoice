///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Link.Domain.Roles;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly LinkDbContext _context;

        public RoleRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<GetRoleResponse>> GetRoleAsync(string email)
        {
            var result = await (from u in _context.Users
                                join r in _context.CatalogItems on u.RoleId equals r.Id
                                where u.Email == email
                                select new GetRoleResponse
                                {
                                    RoleId = r.Id,
                                    RoleName = r.Name,
                                    Name = u.Name,
                                    Email = u.Email
                                }).ToListAsync();
            return result;
        }

        public async Task<GetRoleResponse> GetRoleNewUserAsync(string email)
        {
            var result = await (from u in _context.Accounts
                                join R in _context.AccountRoles on u.Id equals R.AccountId
                                join DR in _context.CatalogItems on R.RoleId equals DR.Id
                                where u.Email == email.Trim()
                                select new GetRoleResponse
                                {
                                    RoleId = R.RoleId ?? Guid.Empty,
                                    RoleName = DR.Name,
                                    Name = u.Name,
                                    Email = u.Email
                                }).FirstOrDefaultAsync();
            return result;
        }
    }
}