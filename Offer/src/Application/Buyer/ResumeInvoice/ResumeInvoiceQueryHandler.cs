///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.ResumeInvoice
{
    public sealed class ResumeInvoiceQueryHandler : IRequestHandler<ResumeInvoiceQuery, ErrorOr<ListDataInfo<ResumeInvoiceResponse>>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly ISystem system;

        public ResumeInvoiceQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository, ISystem system)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
            this.system = system;
        }

        public async Task<ErrorOr<ListDataInfo<ResumeInvoiceResponse>>> Handle(ResumeInvoiceQuery query, CancellationToken cancellationToken)
        {
            var userId = this.system.User.Id;
            if (query.numberOffer == 0)
            {
                return Error.Validation(GetErrorDescription(MessageCodes.ParameterEmpty, "NumberOffer"));
            }
            int cnRegister = 1;
            var result = await this.invoiceDispersionRepository.ResumeInvoiceAsync(query.pagination, query.numberOffer, userId);
            result?.Data?.ForEach(x => { x.NroRegister += cnRegister++; });
            return result;
        }
    }
}