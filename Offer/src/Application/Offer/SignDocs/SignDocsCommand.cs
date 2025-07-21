///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.SignDocs
{
    public record class SignDocsCommand(Guid offerId) : IRequest<ErrorOr<SignDocsResponse>>;
}