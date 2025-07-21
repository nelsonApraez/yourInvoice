///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Create
{
    public record CreateOfferCommand(Guid PayerId) : IRequest<ErrorOr<CreateOfferResponse>>;
}