///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.Accounts.Queries;

namespace yourInvoice.Link.Application.Accounts.Validity
{
    public record ValidityQuery() : IRequest<ErrorOr<ValidityResponse>>;
}