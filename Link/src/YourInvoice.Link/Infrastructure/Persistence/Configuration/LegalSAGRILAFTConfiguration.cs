///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalSAGRILAFTConfiguration : IEntityTypeConfiguration<LegalSAGRILAFT>
    {
        public void Configure(EntityTypeBuilder<LegalSAGRILAFT> builder)
        {
            builder.ToTable("LegalSAGRILAFT", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_LegalGeneralInformation).HasColumnName("Id_LegalGeneralInformation");
            builder.Property(e => e.ResponseDetail).HasMaxLength(250).IsUnicode(false);
        }
    }
}