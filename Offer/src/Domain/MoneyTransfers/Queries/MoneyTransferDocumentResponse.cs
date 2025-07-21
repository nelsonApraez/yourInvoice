///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.MoneyTransfers.Queries
{
    public class MoneyTransferDocumentResponse
    {
        public int Count { get; set; }
        public string Total { get; set; }
        public List<MoneyTransferDocumentTableContentResponse> TableContent { get; set; }
    }

    public class MoneyTransferDocumentTableContentResponse
    {
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Bank { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string Total { get; set; }
    }
}