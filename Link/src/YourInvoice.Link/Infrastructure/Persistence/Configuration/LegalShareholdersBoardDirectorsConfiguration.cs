///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalShareholdersBoardDirectorsConfiguration : IEntityTypeConfiguration<LegalShareholderBoardDirector>
    {
        public void Configure(EntityTypeBuilder<LegalShareholderBoardDirector> builder)
        {
            builder.ToTable("LegalShareholdersBoardDirectors", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_LegalGeneralInformation).HasColumnName("Id_LegalGeneralInformation");
        }
    }
}
