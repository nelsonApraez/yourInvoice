///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.SignSuccessDocs
{
    public record class SignSuccessDocsCommand(Guid offerId) : IRequest<ErrorOr<bool>>;
}