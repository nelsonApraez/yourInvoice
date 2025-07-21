///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class OfferConfiguration : IEntityTypeConfiguration<Domain.Offer>
    {
        public void Configure(EntityTypeBuilder<Domain.Offer> builder)
        {
            builder.ToTable("Offer", ConstantDataBase.SchemaOffer);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Consecutive).UseIdentityColumn(1, 1).ValueGeneratedOnAddOrUpdate().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.EndorseLegarAccepted).HasMaxLength(10).IsFixedLength();
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.HasOne(d => d.Payer).WithMany(p => p.Offers).HasForeignKey(d => d.PayerId).HasConstraintName("FK_Offer_Payer").OnDelete(DeleteBehavior.ClientCascade).IsRequired(true);
            builder.HasOne(d => d.User).WithMany(p => p.Offers).HasForeignKey(d => d.UserId).HasConstraintName("FK_Offer_User").OnDelete(DeleteBehavior.ClientCascade).IsRequired(true);
        }
    }
}