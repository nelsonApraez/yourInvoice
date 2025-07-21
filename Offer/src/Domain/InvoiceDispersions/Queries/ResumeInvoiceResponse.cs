///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.InvoiceDispersions.Queries
{
    public class ResumeInvoiceResponse
    {
        public int NroRegister { get; set; }
        public string InvoiceNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string DayRate { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal FutureValue { get; set; }
        public string PayDay { get; set; }
    }
}