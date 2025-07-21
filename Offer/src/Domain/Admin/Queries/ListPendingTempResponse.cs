///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Admin.Queries
{
    public class ListPendingTempResponse
    {
        public int Nro { get; set; }
        public int Offer { get; set; }
        public string NameSaller { get; set; }
        public string NamePayer { get; set; }
        public string Status { get; set; }
        public int PurchasePercentage { get; set; }
        public string ToolTip { get; set; }
        public string TxPurchased { get; set; }
        public DateTime? OperationDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Days { get; set; }
    }
}