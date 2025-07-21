///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Invoices.Queries;

namespace yourInvoice.Offer.Application.Offer.Invoice.List
{
    public record ListInvoiceByOfferQuery(Guid offerId, SearchInfo pagination) : IRequest<ErrorOr<ListDataInfo<InvoiceListResponse>>>;
}