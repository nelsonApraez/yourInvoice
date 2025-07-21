///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Admin.Queries
{
    public class HeaderDetailResponse
    {
        public int NroOffer { get; set; }
        public string PayerName { get; set; }
        public string SellerName { get; set; }
        public long FutureValue { get; set; }
        public Guid? StatusId { get; set; }
        public string Status { get; set; }
    }
}