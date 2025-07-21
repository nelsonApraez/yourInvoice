///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;

namespace yourInvoice.Offer.Application.Buyer.Resume;

public record ResumeQuery(int numberOffer) : IRequest<ErrorOr<ResumeResponse>>;