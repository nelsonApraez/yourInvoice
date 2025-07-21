///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class BankInformationConfiguration : IEntityTypeConfiguration<BankInformation>
    {
        public void Configure(EntityTypeBuilder<BankInformation> builder)
        {
            builder.ToTable("BankInformation", ConstantDataBase.SchemaBinding);

            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.BankProduct).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Id_GeneralInformation).HasColumnName("Id_GeneralInformation");
            builder.Property(e => e.PhoneNumber).HasMaxLength(15).IsUnicode(false);
        }
    }
}