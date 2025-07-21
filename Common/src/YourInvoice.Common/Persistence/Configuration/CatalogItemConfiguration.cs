///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.Entities;

namespace yourInvoice.Common.Persistence.Configuration
{
    public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItemInfo>
    {
        public void Configure(EntityTypeBuilder<CatalogItemInfo> builder)
        {
            builder.ToTable("CatalogItem");
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.ParentId);
            builder.Property(e => e.CatalogName).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Descripton).HasMaxLength(int.MaxValue).IsUnicode(false);
            builder.Property(e => e.Name).HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql($"{ConstDataBase.DateZonePacific}");
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql($"{ConstDataBase.DateZonePacific}");
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.HasOne(d => d.Catalog).WithMany(p => p.CatalogItem).HasForeignKey(d => d.CatalogName).HasConstraintName("FK_CatalogItem_Catalog");
        }
    }
}