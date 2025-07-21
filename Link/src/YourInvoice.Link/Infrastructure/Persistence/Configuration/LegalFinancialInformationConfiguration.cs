using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class LegalFinancialInformationConfiguration : IEntityTypeConfiguration<LegalFinancialInformation>
    {
        public void Configure(EntityTypeBuilder<LegalFinancialInformation> builder)
        {
            builder.ToTable("LegalFinancialInformation", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.Id_LegalGeneralInformation).HasColumnName("Id_LegalGeneralInformation");
            builder.Property(e => e.MonthlyIncome).HasColumnType("numeric(24, 4)");
            builder.Property(e => e.OtherIncome).HasColumnType("numeric(24, 4)");
            builder.Property(e => e.TotalMonthlyIncome).HasColumnType("numeric(24, 4)");
            builder.Property(e => e.TotalMonthlyExpenditures).HasColumnType("numeric(24, 4)");
            builder.Property(e => e.TotalAssets).HasColumnType("numeric(24, 4)");
            builder.Property(e => e.TotalLiabilities).HasColumnType("numeric(24, 4)");
            builder.Property(e => e.DescribeOriginIncome).HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.OperationTypeDetail).HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.AccountNumber).HasMaxLength(40).IsUnicode(false);
            builder.Property(e => e.Bank).HasMaxLength(40).IsUnicode(false);
            builder.Property(e => e.Amount).HasColumnType("numeric(24, 4)");
            builder.Property(e => e.City).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Currency).HasMaxLength(50).IsUnicode(false);
        }
    }
}
