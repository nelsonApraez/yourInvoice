///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.Accounts.Queries;

namespace yourInvoice.Link.Application.Accounts.GetAccount
{
    public record GetAccountQuery(Guid Id) : IRequest<ErrorOr<AccountResponse>>;
}