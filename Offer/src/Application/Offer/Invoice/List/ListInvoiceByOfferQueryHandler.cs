///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Invoices.Queries;

namespace yourInvoice.Offer.Application.Offer.Invoice.List
{
    public sealed class ListInvoiceByOfferQueryHandler : IRequestHandler<ListInvoiceByOfferQuery, ErrorOr<ListDataInfo<InvoiceListResponse>>>
    {
        private readonly IInvoiceRepository repository;

        public ListInvoiceByOfferQueryHandler(IInvoiceRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<ListDataInfo<InvoiceListResponse>>> Handle(ListInvoiceByOfferQuery command, CancellationToken cancellationToken)
        {
            var result = await this.repository.ListAsync(command.offerId, command.pagination);

            return result;
        }
    }
}