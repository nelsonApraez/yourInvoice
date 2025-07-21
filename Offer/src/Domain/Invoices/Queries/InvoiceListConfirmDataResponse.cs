///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Invoices.Queries
{
    public class InvoiceListConfirmDataResponse
    {
        public Guid Id { get; set; }
        public int Nro { get; set; }
        public int ConsecutiveOffer { get; set; }
        public decimal ValueIva { get; set; }
        public string InvoiceNumber { get; set; }
        public string Status { get; set; }
        public Guid StatusId { get; set; }
        public decimal? Value { get; set; }
        public string DueDate { get; set; }
        public DateTime? DueDateOrder { get; set; }
        public decimal? NegotiationTotal { get; set; }
        public string NegotiationDate { get; set; }
        public DateTime? NegotiationDateOrder { get; set; }
    }
}