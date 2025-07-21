///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Admin.Header
{
    public sealed class HeaderDetailQueryHandler : IRequestHandler<HeaderDetailQuery, ErrorOr<HeaderDetailResponse>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;

        public HeaderDetailQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
        }

        public async Task<ErrorOr<HeaderDetailResponse>> Handle(HeaderDetailQuery query, CancellationToken cancellationToken)
        {
            var result = await this.invoiceDispersionRepository.GetHeaderDetailAsync(query.offerId);
            if (result is null)
            {
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));
            }

            return result;
        }
    }
}