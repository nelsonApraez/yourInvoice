///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LinkStatus
{
    public class LinkStatus : AggregateRoot
    {
        public LinkStatus()
        { }

        public LinkStatus(Guid id, Guid idUserLink, Guid statusLinkId)
        {
            Id = id;
            IdUserLink = idUserLink;
            StatusLinkId = statusLinkId;
        }

        public Guid IdUserLink { get;  set; }
        public Guid StatusLinkId { get; set; }
    }
}