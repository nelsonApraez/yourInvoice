///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.Payers;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class PayerConfiguration : IEntityTypeConfiguration<Payer>
    {
        public void Configure(EntityTypeBuilder<Payer> builder)
        {
            builder.ToTable("Payer", ConstantDataBase.SchemaOffer);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Address).HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.City).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.CityTributa).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Description).IsUnicode(false);
            builder.Property(e => e.Email).HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.MailingAddress).HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Nit).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.NitDv).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Phone).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
        }
    }
}