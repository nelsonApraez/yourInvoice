///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.Roles;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Description).HasMaxLength(550).IsUnicode(false);
            builder.Property(e => e.Name).HasMaxLength(150).IsUnicode(false);
        }
    }
}