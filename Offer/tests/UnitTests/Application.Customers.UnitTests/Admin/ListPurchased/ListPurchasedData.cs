///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Admin.Queries;

namespace Application.Customer.UnitTest.Admin.ListPurchased
{
    public static class ListPurchasedData
    {
        public static SearchInfo GetSearchInfo => new SearchInfo
        {
            ColumnOrder = "Offer",
            PageSize = 10,
            OrderType = "asc",
        };

        public static ListDataInfo<ListPurchasedResponse> GetListPurchasedResponse => new ListDataInfo<ListPurchasedResponse>
        {
            Count = 2,
            Data = new List<ListPurchasedResponse>
            {
                new ListPurchasedResponse{ CurrentValue=1000, FutureValue=2000, NamePayer="Nombre Pagador", NameSaller="Nombre vendedor", Nro=1, OperationDate=DateTime.Now.ToString(), Offer=34 },
                new ListPurchasedResponse{ CurrentValue=1000, FutureValue=2000, NamePayer="Nombre Pagador1", NameSaller="Nombre vendedor1", Nro=1, OperationDate=DateTime.Now.ToString(), Offer=35 },
            },
        };
    }
}