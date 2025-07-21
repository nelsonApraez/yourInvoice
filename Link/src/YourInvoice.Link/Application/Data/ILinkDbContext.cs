///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Link.Domain.AccountRoles;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Roles;

namespace yourInvoice.Link.Application.Data
{
    public interface ILinkDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}