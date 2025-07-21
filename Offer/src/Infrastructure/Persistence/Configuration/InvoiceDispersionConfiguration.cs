///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    internal class InvoiceDispersionConfiguration : IEntityTypeConfiguration<InvoiceDispersion>
    {
        public void Configure(EntityTypeBuilder<InvoiceDispersion> builder)
        {
            builder.ToTable("InvoiceDispersion", ConstantDataBase.SchemaBuyer);
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(x => x.OfferNumber).HasColumnName("OfferNumber").IsUnicode(false);
            builder.Property(x => x.BuyerId).HasColumnName("BuyerId").IsUnicode(false);
            builder.Property(x => x.SellerId).HasColumnName("SellerId").IsUnicode(false);
            builder.Property(x => x.PayerId).HasColumnName("PayerId").IsUnicode(false);
            builder.Property(x => x.PurchaseDate).HasColumnName("PurchaseDate").IsUnicode(false);
            builder.Property(x => x.EndDate).HasColumnName("EndDate").IsUnicode(false);
            builder.Property(x => x.TransactionNumber).HasColumnName("TransactionNumber").IsUnicode(false);
            builder.Property(x => x.InvoiceNumber).HasMaxLength(50).HasColumnName("InvoiceNumber").IsUnicode(false);
            builder.Property(x => x.Division).HasMaxLength(5).HasColumnName("Division").IsUnicode(false);
            builder.Property(x => x.Rate).HasPrecision(18, 4).HasColumnName("Rate").IsUnicode(false);
            builder.Property(x => x.OperationDays).HasColumnName("OperationDays").IsUnicode(false);
            builder.Property(x => x.CurrentValue).HasColumnName("CurrentValue").IsUnicode(false).HasColumnType("bigint");
            builder.Property(x => x.FutureValue).HasColumnName("FutureValue").IsUnicode(false).HasColumnType("bigint"); ;
            builder.Property(x => x.Reallocation).HasMaxLength(1).HasColumnName("Reallocation").IsUnicode(false);
            builder.Property(x => x.NewMoney).HasColumnName("NewMoney").IsUnicode(false);
            builder.Property(x => x.StatusId).HasColumnName("StatusId").IsUnicode(false);
            builder.Property(x => x.ExpirationDate).HasColumnName("ExpirationDate").IsUnicode(false);
            builder.Property(x => x.NumberReminder).HasColumnName("NumberReminder").IsUnicode(false);
            builder.Property(x => x.LastReminder).HasColumnName("LastReminder").IsUnicode(false);
            builder.Property(x => x.OperationDate).HasColumnName("OperationDate").IsUnicode(false);
            builder.Property(x => x.ExpectedDate).HasColumnName("ExpectedDate").IsUnicode(false);
            builder.Property(x => x.ParentTransaction).HasColumnName("ParentTransaction").IsUnicode(false);
            builder.Property(x => x.Status).HasColumnName("Status").IsUnicode(false);
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsUnicode(false);
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsUnicode(false);
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn").IsUnicode(false);
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsUnicode(false);
        }
    }
}