///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.MoneyTransfers;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class MoneyTransferConfiguration : IEntityTypeConfiguration<MoneyTransfer>
    {
        public void Configure(EntityTypeBuilder<MoneyTransfer> builder)
        {
            builder.ToTable("MoneyTransfer", ConstantDataBase.SchemaOffer);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.AccountNumber).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.DocumentNumber).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            builder.HasOne(d => d.Offer).WithMany(p => p.MoneyTransfers).HasForeignKey(d => d.OfferId).HasConstraintName("FK_MoneyTransfer_Offer").OnDelete(DeleteBehavior.ClientCascade).IsRequired(true);
        }
    }
}