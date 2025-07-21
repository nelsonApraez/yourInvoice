///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.InvoiceEvents;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class InvoiceEventConfiguration : IEntityTypeConfiguration<InvoiceEvent>
    {
        public void Configure(EntityTypeBuilder<InvoiceEvent> builder)
        {
            builder.ToTable("InvoiceEvent", ConstantDataBase.SchemaOffer);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(x => x.Claim).HasColumnName("Claim");
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Message).HasMaxLength(8000).IsUnicode(false);
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.HasOne(d => d.Invoice).WithMany(p => p.InvoiceEvents).HasForeignKey(d => d.InvoiceId).HasConstraintName("FK_InvoiceEvent_InvoiceEvent").OnDelete(DeleteBehavior.ClientCascade).IsRequired(true);
        }
    }
}