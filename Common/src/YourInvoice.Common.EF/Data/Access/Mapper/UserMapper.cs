///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    public class UserMapper : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("User","oft");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.IntegrationId).HasColumnName("IntegrationId");
            builder.Property(x => x.Aadid).HasMaxLength(50).HasColumnName("Aadid");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.DocumentTypeId).HasMaxLength(50).HasColumnName("DocumentTypeId");
            builder.Property(x => x.DocumentNumber).HasMaxLength(50).HasColumnName("DocumentNumber");
            builder.Property(x => x.DocumentExpedition).HasMaxLength(50).HasColumnName("DocumentExpedition");
            builder.Property(x => x.Job).HasMaxLength(50).HasColumnName("Job");
            builder.Property(x => x.Address).HasMaxLength(50).HasColumnName("Address");
            builder.Property(x => x.Phone).HasMaxLength(50).HasColumnName("Phone");
            builder.Property(x => x.Email).HasMaxLength(50).HasColumnName("Email");
            builder.Property(x => x.City).HasMaxLength(50).HasColumnName("City");
            builder.Property(x => x.UserTypeId).HasMaxLength(50).HasColumnName("UserTypeId");
            builder.Property(x => x.RoleId).HasMaxLength(50).HasColumnName("RoleId");
            builder.Property(x => x.Company).HasMaxLength(50).HasColumnName("Company");
            builder.Property(x => x.CompanyNit).HasMaxLength(50).HasColumnName("CompanyNit");
            builder.Property(x => x.CompanyNitDv).HasMaxLength(50).HasColumnName("CompanyNitDv");
            builder.Property(x => x.CompanyCommercialRegistrationNumber).HasMaxLength(50).HasColumnName("CompanyCommercialRegistrationNumber");
            builder.Property(x => x.CompanyCommercialRegistrationCity).HasMaxLength(50).HasColumnName("CompanyCommercialRegistrationCity");
            builder.Property(x => x.CompanyChamberOfCommerceCity).HasMaxLength(50).HasColumnName("CompanyChamberOfCommerceCity");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}