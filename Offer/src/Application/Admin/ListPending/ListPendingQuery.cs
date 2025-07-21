///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Admin.Queries;

namespace yourInvoice.Offer.Application.Admin.ListPending
{
    public record ListPendingQuery(SearchInfo pagination) : IRequest<ErrorOr<ListDataInfo<ListPendingResponse>>>;
}