///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.EmailToAdminLinkingCompleted;

public record EmailLinkingCompletedCommand(Guid accountId) : INotification;