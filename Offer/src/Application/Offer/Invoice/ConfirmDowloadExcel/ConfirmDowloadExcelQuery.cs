///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.ConfirmDowloadExcel
{
    public record ConfirmDowloadExcelQuery(Guid offerId) : IRequest<ErrorOr<byte[]>>;
}