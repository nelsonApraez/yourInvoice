///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Invoices.Queries;

namespace yourInvoice.Offer.Application.Offer.Invoice.ListEvents
{
    public sealed class ListInvoiceEventsByOfferQueryHandler : IRequestHandler<ListInvoiceEventsByOfferQuery, ErrorOr<ListDataInfo<InvoiceListEventsResponse>>>
    {
        private readonly IInvoiceRepository repository;

        public ListInvoiceEventsByOfferQueryHandler(IInvoiceRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<ListDataInfo<InvoiceListEventsResponse>>> Handle(ListInvoiceEventsByOfferQuery command, CancellationToken cancellationToken)
        {
            var result = await this.repository.ListEventsAsync(command.offerId, command.pagination);

            return result;
        }
    }
}