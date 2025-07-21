///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Offers.Queries;

namespace yourInvoice.Offer.Application.Offer.Detail;

public record DetailOfferQuery(Guid offerId) : IRequest<ErrorOr<DetailOfferResponse>>;