///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Admin.Queries
{
    public class HeaderTransactionResponse
    {
        public string SellerName { get; set; }
        public string PayerName { get; set; }
        public int TransactioNumber { get; set; }
        public string SellerEmail { get; set; }
        public Guid StatusId { get; set; }
        public string Date { get; set; }
    }
}