///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Offers.Queries;

namespace yourInvoice.Offer.Application.Offer.GetById
{
    public record GetOfferByIdQuery(Guid Id) : IRequest<ErrorOr<GetOfferResponse>>;
}