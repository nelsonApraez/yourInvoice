///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.DianFyMFiles;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class DianFyMFileConfiguration : IEntityTypeConfiguration<DianFyMFile>
    {
        public void Configure(EntityTypeBuilder<DianFyMFile> builder)
        {
            builder.ToTable("DianFyMFile", ConstantDataBase.SchemaBuyer);
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(x => x.Offer).HasColumnName("Offer").IsUnicode(false);
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(200).IsUnicode(false);
            builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(5000).IsUnicode(false);
            builder.Property(x => x.PathStorage).HasColumnName("PathStorage").IsUnicode(false);
            builder.Property(x => x.CountRegisterFile).HasColumnName("CountRegisterFile").IsUnicode(false);
            builder.Property(x => x.Status).HasColumnName("Status").IsUnicode(false);
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsUnicode(false);
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsUnicode(false);
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn").IsUnicode(false);
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsUnicode(false);
        }
    }
}