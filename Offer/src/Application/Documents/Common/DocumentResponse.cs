///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Documents.Common
{
    public record DocumentResponse(
            Guid Id,
            Guid? OfferId,
            Guid? InvoiceId,
            string Name,
            Guid? TypeId,
            bool? IsSigned,
            string Url,
            string Format);
}