///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.DianFyM.Email
{
    public class EmailToSellerPostProcessDianCommand : INotification
    {
        public Guid OfferId { get; set; }
        public int Consecutive { get; set; }
        public Dictionary<string, string> AttachData { get; set; }
    }
}