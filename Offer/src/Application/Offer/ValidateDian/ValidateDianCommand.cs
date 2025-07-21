///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.ValidateDian
{
    public record ValidateDianCommand(Guid Id) : IRequest<ErrorOr<bool>>;
}