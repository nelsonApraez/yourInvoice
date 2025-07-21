///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Admin.Queries;

namespace yourInvoice.Offer.Application.Admin.Header
{
    public record HeaderDetailQuery(int offerId) : IRequest<ErrorOr<HeaderDetailResponse>>;
}