///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Offer.Domain.EventNotifications;

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public class EventNotificationConfiguration : IEntityTypeConfiguration<EventNotification>
    {
        public void Configure(EntityTypeBuilder<EventNotification> builder)
        {
            builder.ToTable("EventNotification", ConstantDataBase.SchemaOffer);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Body).HasMaxLength(32672).IsUnicode(false);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.ModifiedOn).HasDefaultValueSql(ConstantDataBase.DateTimeZone);
            builder.Property(e => e.Status).HasDefaultValueSql("((1))");
            builder.Property(e => e.To).HasMaxLength(150).IsUnicode(false);
            builder.HasOne(d => d.Offer).WithMany(p => p.EventNotifications).HasForeignKey(d => d.OfferId).HasConstraintName("FK_EventNotification_Offer").OnDelete(DeleteBehavior.ClientCascade).IsRequired(true);
        }
    }
}