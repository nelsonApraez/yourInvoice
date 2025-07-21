
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.EmailToAdminLinkingCompletedLegal;

public record EmailLinkingCompletedLegalCommand(Guid accountId) : INotification;
