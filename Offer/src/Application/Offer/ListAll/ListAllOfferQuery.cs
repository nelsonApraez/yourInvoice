///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Offers.Queries;

namespace yourInvoice.Offer.Application.Offer.ListAll;

public record ListAllOfferQuery(SearchInfo pagination) : IRequest<ErrorOr<ListDataInfo<ListAllOfferResponse>>>;