///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class ExposureInformationConfiguration : IEntityTypeConfiguration<ExposureInformation>
    {
        public void Configure(EntityTypeBuilder<ExposureInformation> builder)
        {
            builder.ToTable("ExposureInformation", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_GeneralInformation).HasColumnName("Id_GeneralInformation");
            builder.Property(e => e.ResponseDetail).HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.DeclarationOriginFunds).HasMaxLength(250).IsUnicode(false);
        }
    }
}