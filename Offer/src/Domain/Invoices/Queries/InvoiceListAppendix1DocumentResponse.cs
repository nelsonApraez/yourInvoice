///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Invoices.Queries
{
    public class InvoiceListAppendix1DocumentResponse
    {
        public string InvoiceNumber { get; set; }
        public string Cufe { get; set; }
        public string SellerName { get; set; }
        public string PayerName { get; set; }
        public string EmitDate { get; set; }
        public string DueDate { get; set; }
        public string NegotiationDate { get; set; }
        public decimal NegotiationTotal { get; set; }
        public string NitPayer { get; set; }
    }
}