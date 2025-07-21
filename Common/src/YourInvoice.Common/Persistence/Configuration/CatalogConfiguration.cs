///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.Entities;

namespace yourInvoice.Common.Persistence.Configuration
{
    public class CatalogConfiguration : IEntityTypeConfiguration<CatalogInfo>
    {
        public void Configure(EntityTypeBuilder<CatalogInfo> builder)
        {
            builder.HasKey(e => e.Name).HasName("PK_Catalog_Name");

            builder.ToTable("Catalog");
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CreatedOn).HasDefaultValueSql($"{ConstDataBase.DateZonePacific}");
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql($"{ConstDataBase.DateZonePacific}");
            builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.Property(e => e.Descripton).HasMaxLength(250).IsUnicode(false);
        }
    }
}