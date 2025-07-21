///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.EconomicActivities;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class EconomicActivityConfiguration : IEntityTypeConfiguration<EconomicActivity>
    {
        public void Configure(EntityTypeBuilder<EconomicActivity> builder)
        {
            builder.ToTable("EconomicActivities", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Code).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Description).HasMaxLength(200).IsUnicode(false);
        }
    }
}