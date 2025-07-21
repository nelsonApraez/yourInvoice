//*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetFinancialResponse

    {
        public Guid Id { get; set; }
        public Guid? Id_GeneralInformation { get; set; }
        public string TotalAssets { get; set; }
        public string TotalLiabilities { get; set; }
        public string TotalWorth { get; set; }
        public string MonthlyIncome { get; set; }
        public string MonthlyExpenditures { get; set; }
        public string OtherIncome { get; set; }
        public string DescribeOriginIncome { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }

    }
}