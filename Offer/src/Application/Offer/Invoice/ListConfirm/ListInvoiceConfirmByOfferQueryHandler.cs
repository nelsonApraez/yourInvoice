///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Invoices.Queries;

namespace yourInvoice.Offer.Application.Offer.Invoice.ListConfirm
{
    public sealed class ListInvoiceConfirmByOfferQueryHandler : IRequestHandler<ListInvoiceConfirmByOfferQuery, ErrorOr<ListDataInfo<InvoiceListConfirmDataResponse>>>
    {
        private readonly IInvoiceRepository repository;

        public ListInvoiceConfirmByOfferQueryHandler(IInvoiceRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<ListDataInfo<InvoiceListConfirmDataResponse>>> Handle(ListInvoiceConfirmByOfferQuery command, CancellationToken cancellationToken)
        {
            var result = await this.repository.ListConfirmAsync(command.offerId, command.pagination);
            int cnNro = 1;
            result?.Data?.ForEach(x => x.Nro = cnNro++);

            return result;
        }
    }
}