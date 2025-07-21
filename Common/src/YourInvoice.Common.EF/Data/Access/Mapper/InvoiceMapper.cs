///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    internal class InvoiceMapper : IEntityTypeConfiguration<InvoiceInfo>
    {
        public void Configure(EntityTypeBuilder<InvoiceInfo> builder)
        {
            builder.ToTable("Invoice", "OFT");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.OfferId).IsRequired().HasColumnName("OfferId");
            builder.Property(x => x.Number).HasMaxLength(50).HasColumnName("Number");
            builder.Property(x => x.ZipName).HasMaxLength(100).HasColumnName("ZipName");
            builder.Property(x => x.CUFE).HasMaxLength(150).HasColumnName("CUFE");
            builder.Property(x => x.StatusId).HasColumnName("StatusId");
            builder.Property(x => x.EmitDate).HasColumnName("EmitDate");
            builder.Property(x => x.DueDate).HasColumnName("DueDate");
            builder.Property(x => x.Total).HasColumnName("Total");
            builder.Property(x => x.MoneyTypeId).HasColumnName("MoneyTypeId");
            builder.Property(x => x.Trm).HasColumnName("Trm");
            builder.Property(x => x.ErrorMessage).HasMaxLength(250).HasColumnName("ErrorMessage");
            builder.Property(x => x.NegotiationDate).HasColumnName("NegotiationDate");
            builder.Property(x => x.NegotiationTotal).HasColumnName("NegotiationTotal");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}