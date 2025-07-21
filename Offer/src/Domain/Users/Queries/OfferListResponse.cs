///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Users.Queries
{
    public class OfferListResponse
    {
        public Guid OfferId { get; set; }
        public int OfferNumber { get; set; }
        public int TransactionNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string SellerName { get; set; }
        public string PayerName { get; set; }
        public string CreationDate { get; set; }
        public string EndDate { get; set; }
        public string Rate { get; set; }
        public string Term { get; set; }
        public long PurchaseValue { get; set; }
        public long FutureValue { get; set; }
        public string Status { get; set; }
        public Guid StatusId { get; set; }
        public bool NewMoney { get; set; }
        public Guid? DocId { get; set; }
        public bool HasDocument { get; set; }
        public DateTime OrderDate { get; set; }
    }
}