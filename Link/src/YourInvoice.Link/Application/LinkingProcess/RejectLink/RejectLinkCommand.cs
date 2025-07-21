///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.RejectLink
{
    public record RejectLinkCommand(Guid IdUserLink) : IRequest<ErrorOr<bool>>;
}