///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalBoardDirectorsConfiguration : IEntityTypeConfiguration<LegalBoardDirector>
    {
        public void Configure(EntityTypeBuilder<LegalBoardDirector> builder)
        {
            builder.ToTable("LegalBoardDirectors", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_LegalGeneralInformation).HasColumnName("Id_LegalGeneralInformation");
            builder.Property(e => e.FullNameCompanyName).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.DocumentNumber).HasMaxLength(12).IsUnicode(false);
            builder.Property(e => e.PhoneNumber).HasMaxLength(15).IsUnicode(false);
        }
    }
}
