///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    internal class InvoiceEventMapper : IEntityTypeConfiguration<InvoiceEventInfo>
    {
        public void Configure(EntityTypeBuilder<InvoiceEventInfo> builder)
        {
            builder.ToTable("InvoiceEvent", "OFT");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.InvoiceId).IsRequired().HasColumnName("InvoiceId");
            builder.Property(x => x.Event030).HasColumnName("Event030");
            builder.Property(x => x.Event032).HasColumnName("Event032");
            builder.Property(x => x.Event033).HasColumnName("Event033");
            builder.Property(x => x.Event036).HasColumnName("Event036");
            builder.Property(x => x.Event037).HasColumnName("Event037");
            builder.Property(x => x.Message).HasMaxLength(250).HasColumnName("Message");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
            builder.Property(x => x.Claim).HasColumnName("Claim");
        }
    }
}