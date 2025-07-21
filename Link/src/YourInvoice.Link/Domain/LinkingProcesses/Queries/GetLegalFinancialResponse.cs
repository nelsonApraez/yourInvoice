namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetLegalFinancialResponse
    {
        public Guid Id { get; set; }
        public Guid? Id_LegalGeneralInformation { get; set; }
        public long TotalAssets { get; set; }
        public long TotalLiabilities { get; set; }
        public long TotalMonthlyIncome { get; set; }
        public long MonthlyIncome { get; set; }
        public long TotalMonthlyExpenditures { get; set; }
        public long OtherIncome { get; set; }
        public string DescribeOriginIncome { get; set; }
        public Guid? OperationsForeignCurrencyQuestionId { get; set; }
        public Guid? OperationsForeignCurrency { get; set; }
        public List<Guid?> OperationsTypes { get; set; }
        public string OperationsTypeName { get; set; }
        public string OperationTypeDetail { get; set; }
        public Guid? AccountsForeignCurrencyQuestionId { get; set; }
        public Guid? AccountsForeignCurrency { get; set; }        
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public long? Amount { get; set; }
        public string City { get; set; }
        public string Currency { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }
    }
}
