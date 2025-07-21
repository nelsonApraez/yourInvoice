///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Admin.Queries;

namespace yourInvoice.Offer.Application.Admin.HeaderTransaction
{
    public class HeaderTransactionQueryHandler : IRequestHandler<HeaderTransactionQuery, ErrorOr<HeaderTransactionResponse>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;

        public HeaderTransactionQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
        }

        public async Task<ErrorOr<HeaderTransactionResponse>> Handle(HeaderTransactionQuery query, CancellationToken cancellationToken)
        {
            return await this.invoiceDispersionRepository.GetHeaderTransactionAsync(query.trasactionId);
        }
    }
}