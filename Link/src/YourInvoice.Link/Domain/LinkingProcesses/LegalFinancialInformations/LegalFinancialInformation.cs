using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations
{
    public class LegalFinancialInformation : AggregateRoot
    {
        public LegalFinancialInformation()
        {
        }

        public LegalFinancialInformation(Guid id,
            Guid? idLegalGeneralInformation,
            long totalAssets,
            long totalLiabilities,
            long totalMonthlyIncome,
            long monthlyIncome,
            long monthlyExpenditures,
            long otherIncome,
            string describeOriginIncome,
            Guid? operationsForeignCurrency,
            string operationsType,
            string operationTypeDetail,
            Guid? accountsForeignCurrency,
            string accountNumber,
            string bank,
            long? amount,
            string city,
            string currency,
            Guid? completed,
            DateTime? createdOn,
            Guid? statusId,
            DateTime? statusDate)
        {
            Id = id;
            Id_LegalGeneralInformation = idLegalGeneralInformation;
            TotalAssets = totalAssets;
            TotalLiabilities = totalLiabilities;
            TotalMonthlyIncome = totalMonthlyIncome;
            MonthlyIncome = monthlyIncome;
            TotalMonthlyExpenditures = monthlyExpenditures;
            OtherIncome = otherIncome;
            DescribeOriginIncome = describeOriginIncome;
            OperationsForeignCurrency = operationsForeignCurrency;
            OperationsType = operationsType;
            OperationTypeDetail = operationTypeDetail;
            AccountsForeignCurrency = accountsForeignCurrency;
            AccountNumber = accountNumber;
            Bank = bank;
            Amount = amount;
            City = city;
            Currency = currency;
            Completed = completed;
            CreatedOn = createdOn;
            StatusId = statusId;
            StatusDate = statusDate;
            CreatedOn = createdOn;
        }

        public Guid? Id_LegalGeneralInformation { get; set; }
        public long TotalAssets { get; set; }
        public long TotalLiabilities { get; set; }
        public long TotalMonthlyIncome { get; set; }
        public long MonthlyIncome { get; set; }
        public long TotalMonthlyExpenditures { get; set; }
        public long OtherIncome { get; set; }
        public string DescribeOriginIncome { get; set; }
        public Guid? OperationsForeignCurrency { get; set; }
        public string OperationsType { get; set; }
        public string OperationTypeDetail { get; set; }
        public Guid? AccountsForeignCurrency { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public long? Amount { get; set; }
        public string City { get; set; }
        public string Currency { get; set; }
        public Guid? Completed { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}
