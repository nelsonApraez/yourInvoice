///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Admin.EmailToSeller
{
    public class EmailToSellerAdminPurchasedCommand : INotification
    {
        public Dictionary<string, string> AttachData { get; set; }

        public List<AttachFile> AttachFilesData { get; set; }

        public int NumberOffer { get; set; }
        public string NameSeller { get; set; }

        public List<string> EmailsSeller { get; set; }
    }
}