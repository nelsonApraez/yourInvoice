///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Admin.Queries;

namespace yourInvoice.Offer.Application.Admin.HeaderOffer;
public record HeaderOfferQuery(int offerId) : IRequest<ErrorOr<HeaderTransactionResponse>>;