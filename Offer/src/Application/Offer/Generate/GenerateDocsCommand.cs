///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Generate
{
    public record class GenerateDocsCommand(Guid offerId) : IRequest<ErrorOr<bool>>;
}