///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    internal class OfferMapper : IEntityTypeConfiguration<OfferInfo>
    {
        public void Configure(EntityTypeBuilder<OfferInfo> builder)
        {
            builder.ToTable("Offer", "OFT");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Consecutive).IsRequired().HasColumnName("Consecutive");
            builder.Property(x => x.PayerId).IsRequired().HasColumnName("PayerId");
            builder.Property(x => x.UserId).IsRequired().HasColumnName("UserId");
            builder.Property(x => x.StartDate).HasColumnName("StartDate");
            builder.Property(x => x.EndDate).HasColumnName("EndDate");
            builder.Property(x => x.EndorseLegarAccepted).HasMaxLength(10).HasColumnName("EndorseLegarAccepted");
            builder.Property(x => x.StatusId).HasColumnName("StatusId");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}