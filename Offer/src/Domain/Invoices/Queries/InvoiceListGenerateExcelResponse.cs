///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Invoices.Queries
{
    public class InvoiceListGenerateExcelResponse : InvoiceListAppendix1DocumentResponse
    {
        public string SellerNit { get; set; }
        public string PayerNit { get; set; }
        public int OfferConsecutive { get; set; }
        public string DocumentType { get; set; }
    }
}