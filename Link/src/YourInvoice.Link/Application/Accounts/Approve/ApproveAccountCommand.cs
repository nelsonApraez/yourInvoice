///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.Accounts.Approve;

public record ApproveAccountCommand(Guid accountId) : IRequest<ErrorOr<bool>>;