namespace yourInvoice.Link.Application.LinkingProcess.CreateFinancial
{
    public class Financial
    {
        public Guid Id_GeneralInformation { get; set; }
        public long TotalAssets { get; set; }
        public long TotalLiabilities { get; set; }
        public long TotalWorth { get; set; }
        public long MonthlyIncome { get; set; }
        public long MonthlyExpenditures { get; set; }
        public long OtherIncome { get; set; }
        public string DescribeOriginIncome { get; set; }
        public Guid? Completed { get; set; }

    }
}
