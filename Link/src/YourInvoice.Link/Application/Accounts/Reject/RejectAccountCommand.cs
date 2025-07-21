///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.Accounts.Reject
{
    public record RejectAccountCommand(Guid Id) : IRequest<ErrorOr<bool>>;
}