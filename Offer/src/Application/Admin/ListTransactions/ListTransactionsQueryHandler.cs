///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace yourInvoice.Offer.Application.Admin.ListTransactions
{
    public class ListTransactionsQueryHandler : IRequestHandler<ListTransactionsQuery, ErrorOr<List<ListTransactionsResponse>>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;

        public ListTransactionsQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
        }

        public async Task<ErrorOr<List<ListTransactionsResponse>>> Handle(ListTransactionsQuery query, CancellationToken cancellationToken)
        {
            return await this.invoiceDispersionRepository.ListTransactionsAsync(query.trasactionId);
        }
    }
}