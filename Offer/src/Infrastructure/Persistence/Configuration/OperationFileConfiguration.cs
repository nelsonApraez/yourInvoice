///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.OperationFiles;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class OperationFileConfiguration : IEntityTypeConfiguration<OperationFile>
    {
        public void Configure(EntityTypeBuilder<OperationFile> builder)
        {
            builder.ToTable("OperationFile", ConstantDataBase.SchemaBuyer);
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(200).IsUnicode(false);
            builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(5000).IsUnicode(false);
            builder.Property(x => x.StartDate).HasColumnName("StartDate").IsUnicode(false);
            builder.Property(x => x.EndDate).HasColumnName("EndDate").IsUnicode(false);
            builder.Property(x => x.Status).HasColumnName("Status").IsUnicode(false);
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsUnicode(false);
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsUnicode(false);
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn").IsUnicode(false);
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsUnicode(false);
        }
    }
}