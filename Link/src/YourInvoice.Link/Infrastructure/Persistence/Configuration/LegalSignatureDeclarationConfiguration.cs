///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalSignatureDeclarationConfiguration : IEntityTypeConfiguration<LegalSignatureDeclaration>
    {
        public void Configure(EntityTypeBuilder<LegalSignatureDeclaration> builder)
        {
            builder.ToTable("LegalSignatureDeclaration", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_LegalGeneralInformation).HasColumnName("Id_LegalGeneralInformation");
            builder.Property(e => e.CommitmentAcceptRiskManagement).HasColumnType("bit");
            builder.Property(e => e.ResponsivilityForInformation).HasColumnType("bit");
            builder.Property(e => e.VisitAuthorization).HasColumnType("bit");
            builder.Property(e => e.Statements).HasColumnType("bit");
        }
    }
}