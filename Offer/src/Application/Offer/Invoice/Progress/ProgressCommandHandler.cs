///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.Extensions.Caching.Memory;
using yourInvoice.Common.Entities;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Invoice.Progress
{
    public class ProgressCommandHandler : IRequestHandler<ProgressCommand, ErrorOr<InvoiceProcessCache>>
    {
        private readonly IMemoryCache _memoryCache;

        public ProgressCommandHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<ErrorOr<InvoiceProcessCache>> Handle(ProgressCommand command, CancellationToken cancellationToken)
        {
            InvoiceProcessCache result = _memoryCache.Get<InvoiceProcessCache>(command.offerId);

            if (result == null)
            {
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));
            }

            return result;
        }
    }
}