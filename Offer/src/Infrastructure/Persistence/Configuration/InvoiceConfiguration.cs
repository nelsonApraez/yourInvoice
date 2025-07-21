///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.Invoices;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoice", ConstantDataBase.SchemaOffer);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Cufe).HasMaxLength(150).IsUnicode(false).HasColumnName("CUFE");
            builder.Property(e => e.ErrorMessage).HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.NegotiationTotal).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Number).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.Trm).HasColumnType("money");
            builder.Property(e => e.ZipName).HasMaxLength(100).IsUnicode(false);
            builder.HasOne(d => d.Offer).WithMany(p => p.Invoices).HasForeignKey(d => d.OfferId).HasConstraintName("FK_Invoice_Offer").IsRequired(true);
        }
    }
}