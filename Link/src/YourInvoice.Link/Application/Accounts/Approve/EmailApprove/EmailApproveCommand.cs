///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.Accounts.Approve.EmailApprove
{
    public record EmailApproveCommand : INotification
    {
        public Dictionary<string, string> AttachData { get; set; }
        public string EmailApprove { get; set; }
    }
}