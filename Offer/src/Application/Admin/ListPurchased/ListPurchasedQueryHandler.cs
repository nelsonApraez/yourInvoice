///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace yourInvoice.Offer.Application.Admin.ListPurchased
{
    public sealed class ListPurchasedQueryHandler : IRequestHandler<ListPurchasedQuery, ErrorOr<ListDataInfo<ListPurchasedResponse>>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;

        public ListPurchasedQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository;
        }

        public async Task<ErrorOr<ListDataInfo<ListPurchasedResponse>>> Handle(ListPurchasedQuery query, CancellationToken cancellationToken)
        {
            var result = await this.invoiceDispersionRepository.ListPurchasedAsync(query.pagination);

            if (result is not null && result.Count > 0)
            {
                int cnNro = 1;
                result.Data.ToList().ForEach(x => x.Nro = cnNro++);
            }

            return result;
        }
    }
}