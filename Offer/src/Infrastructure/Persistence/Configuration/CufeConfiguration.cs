///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.Cufes;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class CufeConfiguration : IEntityTypeConfiguration<Cufe>
    {
        public void Configure(EntityTypeBuilder<Cufe> builder)
        {
            builder.ToTable("Cufe", ConstantDataBase.SchemaDbo);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.CufeValue).HasMaxLength(150).IsUnicode(false);
        }
    }
}