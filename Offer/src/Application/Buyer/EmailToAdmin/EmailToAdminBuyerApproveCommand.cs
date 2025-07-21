///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.EmailToAdmin
{
    public class EmailToAdminBuyerApproveCommand : INotification
    {
        public Dictionary<string, string> AttachData { get; set; }

        public int NumberOffer { get; set; }
        public string NameBuyer { get; set; }
    }
}