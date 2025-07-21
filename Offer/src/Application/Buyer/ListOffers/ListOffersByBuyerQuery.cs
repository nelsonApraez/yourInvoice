///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Offer.Application.Buyer.ListOffers
{
    public record ListOffersByBuyerQuery(SearchInfo pagination, bool isHistory) : IRequest<ErrorOr<ListDataInfo<OfferListResponse>>>;
}