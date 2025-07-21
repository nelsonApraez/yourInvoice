///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    internal class DocumentMapper : IEntityTypeConfiguration<DocumentInfo>
    {
        public void Configure(EntityTypeBuilder<DocumentInfo> builder)
        {
            builder.ToTable("Document", "OFT");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.OfferId).IsRequired().HasColumnName("OfferId");
            builder.Property(x => x.InvoiceId).IsRequired().HasColumnName("InvoiceId");
            builder.Property(x => x.TypeId).HasColumnName("TypeId");
            builder.Property(x => x.IsSigned).HasColumnName("IsSigned");
            builder.Property(x => x.Url).HasMaxLength(150).HasColumnName("Url");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}