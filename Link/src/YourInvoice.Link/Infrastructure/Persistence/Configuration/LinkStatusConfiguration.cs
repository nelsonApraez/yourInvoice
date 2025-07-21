///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LinkStatusConfiguration : IEntityTypeConfiguration<LinkStatus>
    {
        public void Configure(EntityTypeBuilder<LinkStatus> builder)
        {
            builder.ToTable("LinkStatus", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        }
    }
}