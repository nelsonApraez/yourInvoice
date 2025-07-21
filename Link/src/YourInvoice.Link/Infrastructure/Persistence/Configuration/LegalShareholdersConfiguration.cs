///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalShareholdersConfiguration : IEntityTypeConfiguration<LegalShareholder>
    {
        public void Configure(EntityTypeBuilder<LegalShareholder> builder)
        {
            builder.ToTable("LegalShareholders", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_LegalGeneralInformation).HasColumnName("Id_LegalGeneralInformation");
            builder.Property(e => e.FullNameCompanyName).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.DocumentNumber).HasMaxLength(12).IsUnicode(false);
            builder.Property(e => e.PhoneNumber).HasMaxLength(15).IsUnicode(false);
        }
    }
}
