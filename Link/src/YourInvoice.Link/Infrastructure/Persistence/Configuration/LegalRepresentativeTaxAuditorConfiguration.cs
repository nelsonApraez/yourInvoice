///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalRepresentativeTaxAuditorConfiguration : IEntityTypeConfiguration<LegalRepresentativeTaxAuditor>
    {
        public void Configure(EntityTypeBuilder<LegalRepresentativeTaxAuditor> builder)
        {
            builder.ToTable("LegalRepresentativeTaxAuditor", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.FirstName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.SecondName).HasMaxLength(40).IsUnicode(false);
            builder.Property(e => e.LastName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.SecondLastName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.DocumentNumber).HasMaxLength(12).IsUnicode(false);
            builder.Property(e => e.ExpeditionCountry).HasMaxLength(150).IsUnicode(false);
            builder.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.HomeAddress).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Phone).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.TaxAuditorFirstName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.TaxAuditorSecondName).HasMaxLength(40).IsUnicode(false);
            builder.Property(e => e.TaxAuditorLastName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.TaxAuditorSecondLastName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.TaxAuditorDocumentNumber).HasMaxLength(12).IsUnicode(false);
            builder.Property(e => e.TaxAuditorPhoneNumber).HasMaxLength(15).IsUnicode(false);
        }
    }
}