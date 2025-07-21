///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.AccountRoles;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class AccountRoleRepository : IAccountRoleRepository
    {
        private readonly LinkDbContext _context;

        public AccountRoleRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public AccountRole Add(AccountRole account) => _context.AccountRoles.Add(account).Entity;
    }
}