///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.Accounts;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Description).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.DigitVerify).HasMaxLength(1).IsUnicode(false).IsFixedLength();
            builder.Property(e => e.DocumentNumber).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.LastName).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.MobileNumber).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Nit).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.PhoneNumber).HasMaxLength(15).IsUnicode(false);
            builder.Property(e => e.SecondLastName).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.SecondName).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.SocialReason).HasMaxLength(100).IsUnicode(false);
        }
    }
}