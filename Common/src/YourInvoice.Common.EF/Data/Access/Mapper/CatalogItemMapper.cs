///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    public class CatalogItemMapper : IEntityTypeConfiguration<CatalogItemInfo>
    {
        public void Configure(EntityTypeBuilder<CatalogItemInfo> builder)
        {
            builder.ToTable("CatalogItem","dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.ParentId).HasColumnName("ParentId");
            builder.Property(x => x.CatalogName).HasMaxLength(100).HasColumnName("CatalogName");
            builder.Property(x => x.Descripton).HasMaxLength(250).HasColumnName("Descripton");
            builder.Property(x => x.Status).IsRequired().HasColumnName("Status");
            builder.Property(x => x.CreatedOn).IsRequired().HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).IsRequired().HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).IsRequired().HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).IsRequired().HasColumnName("ModifiedBy");
        }
    }
}