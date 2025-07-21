///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Admin.Queries
{
    public class ListPurchasedResponse
    {
        public Guid OfferId { get; set; }
        public int Nro { get; set; }
        public int Offer { get; set; }
        public string NameSaller { get; set; }
        public string NamePayer { get; set; }
        public string OperationDate { get; set; }
        public DateTime? OperationDateOrder { get; set; }
        public long CurrentValue { get; set; }
        public long FutureValue { get; set; }
        public bool SummarySent { get; set; }
    }
}