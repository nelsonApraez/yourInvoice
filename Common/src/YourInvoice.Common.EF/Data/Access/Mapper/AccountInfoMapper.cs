///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Access.Mapper
{
    public class AccountInfoMapper : IEntityTypeConfiguration<AccountInfo>
    {
        public void Configure(EntityTypeBuilder<AccountInfo> builder)
        {
            builder.ToTable("Account", "VIN");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.PersonTypeId).HasColumnName("PersonTypeId");
            builder.Property(x => x.Nit).HasColumnName("Nit");
            builder.Property(x => x.DigitVerify).HasColumnName("DigitVerify");
            builder.Property(x => x.SocialReason).HasColumnName("SocialReason");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.SecondName).HasColumnName("SecondName");
            builder.Property(x => x.LastName).HasColumnName("LastName");
            builder.Property(x => x.SecondLastName).HasColumnName("SecondLastName");
            builder.Property(x => x.DocumentTypeId).HasColumnName("DocumentTypeId");
            builder.Property(x => x.DocumentNumber).HasColumnName("DocumentNumber");
            builder.Property(x => x.Email).HasColumnName("Email");
            builder.Property(x => x.MobileNumber).HasColumnName("MobileNumber");
            builder.Property(x => x.MobileCountryId).HasColumnName("MobileCountryId");
            builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber");
            builder.Property(x => x.PhoneCountryId).HasColumnName("PhoneCountryId");
            builder.Property(x => x.ContactById).HasColumnName("ContactById");
            builder.Property(x => x.Description).HasColumnName("Description");
            builder.Property(x => x.StatusId).HasColumnName("StatusId");
            builder.Property(x => x.StatusDate).HasColumnName("StatusDate");
            builder.Property(x => x.Time).HasColumnName("Time");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
            builder.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}