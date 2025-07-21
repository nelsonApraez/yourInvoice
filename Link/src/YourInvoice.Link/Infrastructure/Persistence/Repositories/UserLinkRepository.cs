///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Link.Domain.Users;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class UserLinkRepository : IUserLinkRepository
    {
        private readonly LinkDbContext _context;

        public UserLinkRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsUserAsync(Guid idUser)
        {
            var exist = await _context.Users.AnyAsync(a => a.Id == idUser);
            return exist;
        }

        public async Task<bool> AddAsync(UserInfo user)
        {
            await _context.Users.AddAsync(user);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            await _context.Users.Where(c => c.Id == userId).ExecuteDeleteAsync();

            return true;
        }
    }
}