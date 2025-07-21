///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.MoneyTransfers.Queries
{
    public class BeneficiariesListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Bank { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public decimal Total { get; set; }
    }
}