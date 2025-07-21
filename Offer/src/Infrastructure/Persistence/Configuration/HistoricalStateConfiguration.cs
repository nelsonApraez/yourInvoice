///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.HistoricalStates;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class HistoricalStateConfiguration : IEntityTypeConfiguration<HistoricalState>
    {
        public void Configure(EntityTypeBuilder<HistoricalState> builder)
        {
            builder.ToTable("HistoricalState", ConstantDataBase.SchemaAdmin);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(x => x.StatusId).HasColumnName("StatusId").IsUnicode(false);
            builder.Property(x => x.InvoiceDispersionId).HasColumnName("InvoiceDispersionId").IsUnicode(false);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.HasOne(d => d.Offers).WithMany(p => p.HistoricalStates).HasForeignKey(d => d.OfferId).HasConstraintName("FK_HistoricalState_Offer").IsRequired(true);
        }
    }
}