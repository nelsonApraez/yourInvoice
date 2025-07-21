///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Invoices.Queries
{
    public class InvoiceListEventsResponse : InvoiceListResponse
    {
        public string CreatedOn { get; set; }
        public DateTime? CreatedOnOrder { get; set; }
        public string[] Events { get; set; }
        public string RejectDescription { get; set; }
    }
}