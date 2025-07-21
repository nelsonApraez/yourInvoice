///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations
{
    public class FinancialInformation : AggregateRoot
    {
        public FinancialInformation()
        {
        }

        public FinancialInformation(Guid id,
            Guid? idGeneralInformation,
            long totalAssets,
            long totalLiabilities,
            long totalWorth,
            long monthlyIncome,
            long monthlyExpenditures,
            long otherIncome,
            string describeOriginIncome,
            Guid? completed,
            DateTime? createdOn,
            Guid? statusId,
            DateTime? statusDate)
        {
            Id = id;
            Id_GeneralInformation = idGeneralInformation;
            TotalAssets = totalAssets;
            TotalLiabilities = totalLiabilities;
            TotalWorth = totalWorth;
            MonthlyIncome = monthlyIncome;
            MonthlyExpenditures = monthlyExpenditures;
            OtherIncome = otherIncome;
            DescribeOriginIncome = describeOriginIncome;
            Completed = completed;
            CreatedOn = createdOn;
            StatusId = statusId;
            StatusDate = statusDate;
            CreatedOn = createdOn;
        }

        public Guid? Id_GeneralInformation { get; set; }
        public long TotalAssets { get; set; }
        public long TotalLiabilities { get; set; }
        public long TotalWorth { get; set; }
        public long MonthlyIncome { get; set; }
        public long MonthlyExpenditures { get; set; }
        public long OtherIncome { get; set; }
        public string DescribeOriginIncome { get; set; }
        public Guid? Completed { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}