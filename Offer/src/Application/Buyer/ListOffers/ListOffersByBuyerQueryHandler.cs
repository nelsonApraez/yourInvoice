///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Offer.Application.Buyer.ListOffers
{
    public sealed class ListOffersByBuyerQueryHandler : IRequestHandler<ListOffersByBuyerQuery, ErrorOr<ListDataInfo<OfferListResponse>>>
    {
        private readonly IUserRepository _repository;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly ISystem system;

        public ListOffersByBuyerQueryHandler(IUserRepository repository, ICatalogBusiness catalogBusiness, ISystem system)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public async Task<ErrorOr<ListDataInfo<OfferListResponse>>> Handle(ListOffersByBuyerQuery command, CancellationToken cancellationToken)
        {
            var buyerId = this.system.User.Id;
            int pageSize = command.pagination.PageSize;
            ListDataInfo<OfferListResponse> result;
            if (command.isHistory)
            {
                result = await _repository.ListOffersHistoryAsync(buyerId, command.pagination);
            }
            else
            {
                var timeEnable = await _catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.TimeEnableOffers);
                result = await _repository.ListOffersAsync(buyerId, command.pagination, Convert.ToInt16(timeEnable.Descripton));
            }
            var status = await _catalogBusiness.GetByIdAsync(CatalogCode_InvoiceDispersionStatus.PendingTransfer);
            SetNewStatus(result, command.isHistory, status.Descripton);
            //Se ajusta paginacion despues del ajuste del nuevo set del estado
            var nameColumn = command.pagination.ColumnOrder.UpperFirtsLetter();
            var IsOrderAsc = command.pagination.OrderType.ToLowerInvariant().Equals("asc");
            result.Data = IsOrderAsc ? result.Data.OrderBy(nameColumn).ToList() : result.Data.OrderByDescending(nameColumn).ToList();
            result.Data = result.Data.Skip(command.pagination.StartIndex).Take(pageSize).ToList();

            return result;
        }

        private static void SetNewStatus(ListDataInfo<OfferListResponse> result, bool isHistory, string status)
        {
            List<OfferListResponse> newList = new();

            var offerNumbers = result.Data.Select(x => x.OfferNumber).Distinct().ToList();

            foreach (var offerNumber in offerNumbers)
            {
                var offers = result.Data.Where(x => x.OfferNumber == offerNumber).ToList();
                var offerItem = offers.FirstOrDefault();
                var docId = offers.FirstOrDefault(x => x.DocId != null)?.DocId;
                var requiredNewMoney = offers.Any(x => x.NewMoney == true);

                if (offerItem.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased && requiredNewMoney && docId == null)
                {
                    offerItem.Status = status;
                    offerItem.StatusId = CatalogCode_InvoiceDispersionStatus.PendingTransfer;
                }

                OfferListResponse newResp = new()
                {
                    Status = offerItem.Status,
                    CreationDate = offerItem.CreationDate,
                    DocId = docId,
                    FutureValue = offers.Sum(x => x.FutureValue),
                    NewMoney = offerItem.NewMoney,
                    OfferId = offerItem.OfferId,
                    OfferNumber = offerItem.OfferNumber,
                    PayerName = offerItem.PayerName,
                    PurchaseValue = offers.Sum(x => x.PurchaseValue),
                    Rate = offerItem.Rate,
                    SellerName = offerItem.SellerName,
                    Term = offerItem.Term,
                    StatusId = offerItem.StatusId,
                    HasDocument = offers.Any(x => x.DocId != null),
                    OrderDate = offerItem.OrderDate
                };

                newList.Add(newResp);
            }

            if (!isHistory)
            {
                newList.RemoveAll(x => x.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased);//cuando no es historial se debe eliminar las de estado comprada
            }

            result.Count = newList.Count;
            result.Data = newList;
        }
    }
}