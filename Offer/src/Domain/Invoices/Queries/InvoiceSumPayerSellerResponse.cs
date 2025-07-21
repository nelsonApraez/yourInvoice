///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Invoices.Queries
{
    public class InvoiceSumPayerSellerResponse
    {
        public string PayerName { get; set; }
        public string SellerName { get; set; }
        public string SellerCompany { get; set; }
        public decimal? InvoiceSum { get; set; }
    }
}