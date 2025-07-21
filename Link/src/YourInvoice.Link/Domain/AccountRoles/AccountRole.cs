///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Roles;

namespace yourInvoice.Link.Domain.AccountRoles
{
    public class AccountRole : AggregateRoot
    {
        public AccountRole()
        {
        }

        public AccountRole(Guid id, Guid accountId, Guid roleId)
        {
            Id = id;
            AccountId = accountId;
            RoleId = roleId;
        }

        public Guid Id { get; set; }

        public Guid? AccountId { get; set; }

        public Guid? RoleId { get; set; }

        public virtual Account? Account { get; set; }

        public virtual Role? Role { get; set; }
    }
}