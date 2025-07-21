///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.EmailToAdmin
{
    public record EmailToAdminProcessFileCommand : INotification
    {
        public Dictionary<string, string> AttachData { get; set; }
        public string NameFile { get; set; }
        public string LinkFile { get; set; }

        public string MessageValidationFile { get; set; }
    }
}