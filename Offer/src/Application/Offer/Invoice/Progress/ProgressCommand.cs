///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;

namespace yourInvoice.Offer.Application.Offer.Invoice.Progress
{
    public record ProgressCommand(Guid offerId) : IRequest<ErrorOr<InvoiceProcessCache>>;
}