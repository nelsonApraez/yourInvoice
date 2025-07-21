using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalCommercialAndBankReferenceConfiguration : IEntityTypeConfiguration<LegalCommercialAndBankReference>
    {
        public void Configure(EntityTypeBuilder<LegalCommercialAndBankReference> builder)
        {
            builder.ToTable("LegalCommercialAndBankReference", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_LegalGeneralInformation).HasColumnName("Id_LegalGeneralInformation");
            builder.Property(e => e.PhoneNumberCommercial).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.CommercialReference).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.PhoneNumberBank).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.BankReference).HasMaxLength(100).IsUnicode(false);
        }
    }
}
