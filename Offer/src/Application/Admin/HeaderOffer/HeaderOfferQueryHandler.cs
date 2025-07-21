///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Admin.HeaderOffer
{
    public sealed class HeaderOfferQueryHandler : IRequestHandler<HeaderOfferQuery, ErrorOr<HeaderTransactionResponse>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;

        public HeaderOfferQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
        }

        public async Task<ErrorOr<HeaderTransactionResponse>> Handle(HeaderOfferQuery query, CancellationToken cancellationToken)
        {
            var result = await this.invoiceDispersionRepository.GetHeaderOfferAsync(query.offerId);
            if (result == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            return result;
        }
    }
}