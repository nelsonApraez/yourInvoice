///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Admin.Queries;

namespace Application.Customer.UnitTest.Admin.ListDetail
{
    public static class ListDetailData
    {
        public static SearchInfo GetSearchInfo => new SearchInfo
        {
            ColumnOrder = "Offer",
            PageSize = 10,
            OrderType = "asc",
        };

        public static ListDataInfo<ListDetailResponse> GetListDetailResponse => new ListDataInfo<ListDetailResponse>
        {
            Count = 3,
            Data = new List<ListDetailResponse> {
                 new ListDetailResponse
                 {
                      Nro=1,
                       NroOffer=1,
                        CurrentValue=200000,
                         FutureValue=300000,
                          NameBuyer="Nombre pagador 1",
                           TimeLeft="4 Horas",
                            Rate="0.17%",
                             Status="Pendiente de compra",
                              RateAux=16,
                               ExpirationDate=DateTime.Now,
                 },
                 new ListDetailResponse
                 {
                      Nro=2,
                       NroOffer=2,
                        CurrentValue=200000,
                         FutureValue=300000,
                          NameBuyer="Nombre pagador 2",
                           TimeLeft="4 Horas",
                            Rate="0.17%",
                             Status="Pendiente de compra",
                              RateAux=16,
                               ExpirationDate=DateTime.Now,
                 },
                  new ListDetailResponse
                 {
                      Nro=3,
                       NroOffer=3,
                        CurrentValue=200000,
                         FutureValue=300000,
                          NameBuyer="Nombre pagador 3",
                           TimeLeft="4 Horas",
                            Rate="0.17%",
                             Status="Pendiente de compra",
                              RateAux=16,
                               ExpirationDate=DateTime.Now,
                 }
             },
        };
    }
}