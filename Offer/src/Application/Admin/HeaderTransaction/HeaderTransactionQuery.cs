///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Admin.Queries;

namespace yourInvoice.Offer.Application.Admin.HeaderTransaction;
public record HeaderTransactionQuery(int trasactionId) : IRequest<ErrorOr<HeaderTransactionResponse>>;