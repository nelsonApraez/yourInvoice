///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.GetBase64Document
{
    public record GetBase64DocumentQuery(Guid documentId) : IRequest<ErrorOr<string[]>>;
}