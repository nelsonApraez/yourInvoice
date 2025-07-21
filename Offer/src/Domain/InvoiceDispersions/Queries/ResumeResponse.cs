///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.InvoiceDispersions.Queries
{
    public class ResumeResponse
    {
        public Guid OfferId { get; set; }
        public int NroOffer { get; set; }
        public string NameSaler { get; set; }
        public string NamePayer { get; set; }
        public int OperationDay { get; set; }
        public string Rate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public long CurrentValue { get; set; }
        public long FutureValue { get; set; }
        public long RecordValue { get; set; }
        public string Status { get; set; }
        public bool? NewMoneyOffer { get; set; }
        public bool HasDocument { get; set; }
        public Guid StatusId { get; set; }
        public string OperationDate { get; set; }
        public Dictionary<string, List<ListDocsResponse>> Documents { get; set; }
    }
}