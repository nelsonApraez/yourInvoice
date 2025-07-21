///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.ApproveLink;

public record ApproveLinkCommand(Guid IdUserLink) : IRequest<ErrorOr<bool>>;