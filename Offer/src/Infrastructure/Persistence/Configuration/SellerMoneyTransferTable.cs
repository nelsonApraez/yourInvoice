///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.SellerMoneyTransfers;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class SellerMoneyTransferTable : IEntityTypeConfiguration<SellerMoneyTransfer>
    {
        public void Configure(EntityTypeBuilder<SellerMoneyTransfer> builder)
        {
            builder.ToTable("SellerMoneyTransfer", ConstantDataBase.SchemaOffer);

            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.AccountNumber).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.DocumentNumber).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.HasOne(d => d.User).WithMany(p => p.SellerMoneyTransfers).HasForeignKey(d => d.UserId).HasConstraintName("FK_SellerMoneyTransfer_User").OnDelete(DeleteBehavior.ClientCascade).IsRequired(true);
        }
    }
}