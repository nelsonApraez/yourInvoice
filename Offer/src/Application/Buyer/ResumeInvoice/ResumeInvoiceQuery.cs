///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;

namespace yourInvoice.Offer.Application.Buyer.ResumeInvoice;

public record ResumeInvoiceQuery(SearchInfo pagination, int numberOffer) : IRequest<ErrorOr<ListDataInfo<ResumeInvoiceResponse>>>;