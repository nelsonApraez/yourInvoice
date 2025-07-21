///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.AccountRoles;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
    {
        public void Configure(EntityTypeBuilder<AccountRole> builder)
        {
            builder.ToTable("AccountRole", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(d => d.Account).WithMany(p => p.AccountRoles).HasForeignKey(d => d.AccountId).HasConstraintName("FK_AccountRole_Account");
            builder.HasOne(d => d.Role).WithMany(p => p.AccountRoles).HasForeignKey(d => d.RoleId).HasConstraintName("FK_AccountRole_Role");
        }
    }
}