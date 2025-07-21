///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Invoices.Queries
{
    public class InvoiceListResponse
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string ZipName { get; set; }
        public string DueDate { get; set; }
        public DateTime? DueDateOrder { get; set; }
        public string Status { get; set; }
        public Guid StatusId { get; set; }
        public decimal? Value { get; set; }
        public string Currency { get; set; }
    }
}