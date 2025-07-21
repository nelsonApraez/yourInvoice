///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Admin.Queries;

namespace yourInvoice.Offer.Application.Admin.ListTransactions;
public record ListTransactionsQuery(int trasactionId) : IRequest<ErrorOr<List<ListTransactionsResponse>>>;