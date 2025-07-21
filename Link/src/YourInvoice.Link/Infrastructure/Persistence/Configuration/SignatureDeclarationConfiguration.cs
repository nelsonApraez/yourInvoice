///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class SignatureDeclarationConfiguration : IEntityTypeConfiguration<SignatureDeclaration>
    {
        public void Configure(EntityTypeBuilder<SignatureDeclaration> builder)
        {
            builder.ToTable("SignatureDeclaration", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_GeneralInformation).HasColumnName("Id_GeneralInformation");
            builder.Property(e => e.GeneralStatement).HasColumnType("bit");
            builder.Property(e => e.VisitAuthorization).HasColumnType("bit");
            builder.Property(e => e.SourceFundsDeclaration).HasColumnType("bit");
        }
    }
}