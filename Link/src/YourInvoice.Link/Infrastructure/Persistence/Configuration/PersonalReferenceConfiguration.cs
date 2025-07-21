///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class PersonalReferenceConfiguration : IEntityTypeConfiguration<PersonalReferences>
    {
        public void Configure(EntityTypeBuilder<PersonalReferences> builder)
        {
            builder.ToTable("PersonalReferences", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_GeneralInformation).HasColumnName("Id_GeneralInformation");
            builder.Property(e => e.NameBussines).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.NamePersonalReference).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.PhoneNumber).HasMaxLength(15).IsUnicode(false);
        }
    }
}