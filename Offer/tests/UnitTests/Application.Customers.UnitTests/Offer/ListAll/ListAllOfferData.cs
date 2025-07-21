///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Offers.Queries;

namespace Application.Customer.UnitTest.Offer.ListAll
{
    public static class ListAllOfferData
    {
        public static SearchInfo GetSearchInfo => new SearchInfo
        {
            ColumnOrder = "Status",
            OrderType = "",
            PageSize = 0,
            SortDirection = 0,
            StartIndex = 0,
            filter = ""
        };

        public static ListDataInfo<ListAllOfferResponse> GetListAllOfferResponse =>
            new ListDataInfo<ListAllOfferResponse>()
            {
                Count = 1,
                Data = new List<ListAllOfferResponse>()
                 {
                      new ListAllOfferResponse
                      {
                            DateCreation = "20/Ene/2024",
                            NamePayer = "Nombre del pagador",
                            NoOffer = 222,
                            OfferId = Guid.NewGuid(),
                            Status = "EN PROCESO"
                      }
                 }
            };

        public static ListDataInfo<ListAllOfferResponse> GetListAllOfferResponseNull => null;
    }
}