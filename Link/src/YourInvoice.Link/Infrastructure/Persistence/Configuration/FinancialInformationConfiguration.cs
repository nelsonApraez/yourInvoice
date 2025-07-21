///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;

namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public class FinancialInformationConfiguration : IEntityTypeConfiguration<FinancialInformation>
    {
        public void Configure(EntityTypeBuilder<FinancialInformation> builder)
        {
            builder.ToTable("FinancialInformation", ConstantDataBase.SchemaBinding);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.Property(e => e.DescribeOriginIncome).HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.Id_GeneralInformation).HasColumnName("Id_GeneralInformation");
            builder.Property(e => e.MonthlyExpenditures).HasColumnType("numeric(18, 0)");
            builder.Property(e => e.MonthlyIncome).HasColumnType("numeric(18, 0)");
            builder.Property(e => e.OtherIncome).HasColumnType("numeric(18, 0)");
            builder.Property(e => e.TotalAssets).HasColumnType("numeric(18, 0)");
            builder.Property(e => e.TotalLiabilities).HasColumnType("numeric(18, 0)");
            builder.Property(e => e.TotalWorth).HasColumnType("numeric(18, 0)");
        }
    }
}