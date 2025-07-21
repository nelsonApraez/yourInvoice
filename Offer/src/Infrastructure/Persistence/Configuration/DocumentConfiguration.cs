///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.Documents;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Document", ConstantDataBase.SchemaOffer);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.Property(e => e.Url).HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.RelatedId).IsRequired(false);
            builder.HasOne(d => d.Offer).WithMany(p => p.Document).HasForeignKey(d => d.OfferId).HasConstraintName("FK_Document_Offer").OnDelete(DeleteBehavior.ClientCascade).IsRequired(true);
        }
    }
}