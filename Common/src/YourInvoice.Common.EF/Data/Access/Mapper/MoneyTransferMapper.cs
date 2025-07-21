///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    internal class MoneyTransferMapper : IEntityTypeConfiguration<MoneyTransferInfo>
    {
        public void Configure(EntityTypeBuilder<MoneyTransferInfo> builder)
        {
            builder.ToTable("MoneyTransfer", "OFT");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.OfferId).IsRequired().HasColumnName("OfferId");
            builder.Property(x => x.DocumentTypeId).HasColumnName("DocumentTypeId");
            builder.Property(x => x.DocumentNumber).HasMaxLength(20).HasColumnName("DocumentNumber");
            builder.Property(x => x.BankId).HasColumnName("BankId");
            builder.Property(x => x.AccountTypeId).HasColumnName("AccountTypeId");
            builder.Property(x => x.AccountNumber).HasMaxLength(20).HasColumnName("AccountNumber");
            builder.Property(x => x.Total).HasColumnName("Total");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}