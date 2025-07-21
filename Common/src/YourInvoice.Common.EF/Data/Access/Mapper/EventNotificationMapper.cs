///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    internal class EventNotificationMapper : IEntityTypeConfiguration<EventNotificationInfo>
    {
        public void Configure(EntityTypeBuilder<EventNotificationInfo> builder)
        {
            builder.ToTable("EventNotification", "OFT");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.OfferId).IsRequired().HasColumnName("OfferId");
            builder.Property(x => x.TypeId).HasColumnName("TypeId");
            builder.Property(x => x.Body).HasMaxLength(4000).HasColumnName("Body");
            builder.Property(x => x.To).HasMaxLength(150).HasColumnName("To");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}