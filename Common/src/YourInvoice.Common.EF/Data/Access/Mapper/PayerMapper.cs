///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    internal class PayerMapper : IEntityTypeConfiguration<PayerInfo>
    {
        public void Configure(EntityTypeBuilder<PayerInfo> builder)
        {
            builder.ToTable("Payer", "OFT");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Nit).HasMaxLength(50).HasColumnName("Nit");
            builder.Property(x => x.NitDv).HasMaxLength(50).HasColumnName("NitDv");
            builder.Property(x => x.Address).HasMaxLength(250).HasColumnName("Address");
            builder.Property(x => x.City).HasMaxLength(50).HasColumnName("City");
            builder.Property(x => x.State).HasColumnName("State");
            builder.Property(x => x.CityTributa).HasMaxLength(100).HasColumnName("CityTributa");
            builder.Property(x => x.StateTributa).HasColumnName("StateTributa");
            builder.Property(x => x.Phone).HasMaxLength(20).HasColumnName("Phone");
            builder.Property(x => x.Email).HasMaxLength(200).HasColumnName("Email");
            builder.Property(x => x.MailingAddress).HasMaxLength(200).HasColumnName("MailingAddress");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}