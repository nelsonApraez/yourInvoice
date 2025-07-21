///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Persistence.Configuration;
using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace yourInvoice.Offer.Application.Admin.ListPending
{
    public sealed class ListPendingQueryHandler : IRequestHandler<ListPendingQuery, ErrorOr<ListDataInfo<ListPendingResponse>>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly ICatalogBusiness catalogBusiness;

        public ListPendingQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository, ICatalogBusiness catalogBusiness)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository;
            this.catalogBusiness = catalogBusiness;
        }

        public async Task<ErrorOr<ListDataInfo<ListPendingResponse>>> Handle(ListPendingQuery query, CancellationToken cancellationToken)
        {
            int pageSize = query.pagination.PageSize;
            var offerPending = await this.invoiceDispersionRepository.ListPendingBuysAsync(query.pagination);            
            if (offerPending is not null)
            {
                IEnumerable<CatalogItemInfo> stateOffer = await this.catalogBusiness.ListByCatalogAsync(ConstDataBase.OfferState);
                int cnNro = 1;
                var pending = offerPending.Data.Select(s => new ListPendingResponse
                {
                    Nro = cnNro++,
                    Offer = s.Offer,
                    NameSaller = s.NameSaller,
                    NamePayer = s.NamePayer,
                    StatusId = Guid.Parse(GetStatus(s.Status, s.PurchasePercentage, stateOffer)[0]),
                    Status = GetStatus(s.Status, s.PurchasePercentage, stateOffer)[1],
                    PurchasePercentage = s.PurchasePercentage,
                    ToolTip = s.ToolTip,
                    TxPurchased = s.TxPurchased,
                    OperationDate = s.OperationDate.DateddMMMyyyy(),
                    ExpectedDate = s.ExpectedDate.DateddMMMyyyy(),
                    EndDate = s.EndDate.DateddMMMyyyy(),
                    Days = s.ExpectedDate is null || s.OperationDate is null ? 0 : (Convert.ToDateTime(s.ExpectedDate) - Convert.ToDateTime(s.OperationDate)).Days,
                }).ToList();
                pending = pending.Where(c => c.StatusId != CatalogCode_OfferStatus.Purchased).ToList();
                var filter = query.pagination.filter;
                var nameColumn = query.pagination.ColumnOrder.UpperFirtsLetter();
                var IsOrderAsc = query.pagination.OrderType.ToLowerInvariant().Equals("asc");
                if (string.IsNullOrEmpty(filter))
                {
                    var DataTemp = IsOrderAsc ? pending.OrderBy(nameColumn).ToList() : pending.OrderByDescending(nameColumn).ToList();
                    return new ListDataInfo<ListPendingResponse>
                    {
                        Count = DataTemp.Count,                        
                        Data = DataTemp.Skip(query.pagination.StartIndex).Take(pageSize).ToList()
                    };
                }
                var pendingTemp = pending.Where(c => c.Offer.ToString().Equals(filter) || c.NameSaller.ToLower().Contains(filter.ToLower()) || c.NamePayer.ToLower().Contains(filter.ToLower()) || c.Status.ToLower().ToLower().Contains(filter.ToLower())
                || c.OperationDate.Contains(filter) || c.ExpectedDate.Contains(filter) || c.EndDate.Contains(filter) || c.Days.ToString().Equals(filter)).ToList();
                var DataTemp2 = IsOrderAsc ? pendingTemp.OrderBy(nameColumn).ToList() : pendingTemp.OrderByDescending(nameColumn).ToList();
                return new ListDataInfo<ListPendingResponse>
                {
                    Count = DataTemp2.Count,
                    Data = DataTemp2.Skip(query.pagination.StartIndex).Take(pageSize).ToList()
                };
            }
            return new ListDataInfo<ListPendingResponse>();
        }

        private List<string> GetStatus(string status, int purchasePercentage, IEnumerable<CatalogItemInfo> catalogItems)
        {
            if (!string.IsNullOrEmpty(status))
            {
                var stateOfferEnable = catalogItems.First(c => c.Id == CatalogCode_OfferStatus.Enabled).Name;
                return new List<string> { CatalogCode_OfferStatus.Enabled.ToString(), stateOfferEnable };
            }
            if (purchasePercentage < 100)
            {
                var stateOfferInProgress = catalogItems.First(c => c.Id == CatalogCode_OfferStatus.InProgress).Name;
                return new List<string> { CatalogCode_OfferStatus.InProgress.ToString(), stateOfferInProgress };
            }

            var stateOfferPurchased = catalogItems.First(c => c.Id == CatalogCode_OfferStatus.Purchased).Name;
            return new List<string> { CatalogCode_OfferStatus.Purchased.ToString(), stateOfferPurchased };
        }
    }
}