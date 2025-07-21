///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus
{
    public class ChangeLinkStatusCommand : INotification
    {
        public Guid IdUserLink { get; set; }
        public Guid StatusLinkId { get; set; }
    }
}