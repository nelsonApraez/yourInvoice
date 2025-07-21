///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class GeneralInformationConfiguration : IEntityTypeConfiguration<GeneralInformation>
    {
        public void Configure(EntityTypeBuilder<GeneralInformation> builder)
        {
            builder.ToTable("GeneralInformation", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Address).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.DocumentNumber).HasMaxLength(12).IsUnicode(false);
            builder.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.ExpeditionDate).HasColumnType("datetime");
            builder.Property(e => e.FirstName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.LastName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.MovilPhoneNumber).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.PhoneNumber).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.SecondLastName).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.SecondName).HasMaxLength(40).IsUnicode(false);
            builder.Property(e => e.PhoneCorrespondence).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.AddressCorrespondence).HasMaxLength(100).IsUnicode(false);
        }
    }
}