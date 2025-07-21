///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Admin.ListDetail
{
    public sealed class ListDetailQueryHandler : IRequestHandler<ListDetailQuery, ErrorOr<ListDataInfo<ListDetailResponse>>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;

        public ListDetailQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
        }

        public async Task<ErrorOr<ListDataInfo<ListDetailResponse>>> Handle(ListDetailQuery query, CancellationToken cancellationToken)
        {
            var result = await this.invoiceDispersionRepository.ListDetailAsync(query.offerId, query.pagination);
            if (result is null || result.Count <= 0)
            {
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));
            }
            int cnNro = 1;
            if (query.pagination.ColumnOrder == "timeLeft")
            {
                var IsOrderAsc = query.pagination.OrderType.ToLowerInvariant().Equals("asc");
                var DataTemp = IsOrderAsc ? result.Data.OrderBy("TimeLeftOrder").ToList() : result.Data.OrderByDescending("TimeLeftOrder").ToList();
                var result2 = new ListDataInfo<ListDetailResponse>
                {
                    Count = DataTemp.Count,
                    Data = DataTemp.Skip(query.pagination.StartIndex).Take(query.pagination.PageSize).ToList()
                };

                result2.Data.ForEach(x => x.Nro = cnNro++);
                return result2;
            }

            result.Data.ForEach(x => x.Nro = cnNro++);
            return result;
        }
    }
}