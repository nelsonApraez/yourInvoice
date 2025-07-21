///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;
using yourInvoice.Link.Domain.AccountRoles;

namespace yourInvoice.Link.Domain.Roles
{
    public class Role : AggregateRoot
    {
        public Role()
        {
        }

        public Role(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
    }
}