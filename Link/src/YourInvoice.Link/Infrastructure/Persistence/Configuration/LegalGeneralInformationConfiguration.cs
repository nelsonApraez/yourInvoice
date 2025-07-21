///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalGeneralInformationConfiguration : IEntityTypeConfiguration<LegalGeneralInformation>
    {
        public void Configure(EntityTypeBuilder<LegalGeneralInformation> builder)
        {
            builder.ToTable("LegalGeneralInformation", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Nit).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.CheckDigit).HasMaxLength(1).IsUnicode(false);
            builder.Property(e => e.CompanyName).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.SocietyTypeDetail).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Fee).HasColumnType("numeric(5, 2)");
            builder.Property(e => e.OriginResources).HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.EmailCorporate).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.PhoneNumber).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.EconomicActivityDetail).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.ElectronicInvoiceEmail).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Address).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.BranchAddress).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.BranchPhoneNumber).HasMaxLength(30).IsUnicode(false);
            builder.Property(e => e.BranchContactName).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.BranchDocumentNumber).HasMaxLength(12).IsUnicode(false);
            builder.Property(e => e.BranchContactPhone).HasMaxLength(30).IsUnicode(false);
            builder.Property(e => e.BranchEmailContact).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.BranchPosition).HasMaxLength(100).IsUnicode(false);
        }
    }
}