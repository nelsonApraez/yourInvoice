///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Admin.Queries;

namespace Application.Customer.UnitTest.Admin.ListPending
{
    public static class ListPendingData
    {
        public static SearchInfo GetSearchInfo => new SearchInfo
        {
            ColumnOrder = "Offer",
            PageSize = 10,
            OrderType = "asc",
        };

        public static IEnumerable<CatalogItemInfo> GetCatalogItemInfo => new List<CatalogItemInfo>
        {
          new CatalogItemInfo{ Id=Guid.Parse( "bb421108-ed7f-4ca4-8aed-05e4f6139285"), Name= "Habilitada" },
          new CatalogItemInfo{ Id=Guid.Parse( "51ed6ec6-f55e-4301-8bb8-cd425531856f"), Name= "En Proceso" },
          new CatalogItemInfo{ Id=Guid.Parse( "E23D5024-78B7-465E-95E7-F0509E41AA4E"), Name= "Comprada" },
          new CatalogItemInfo{ Id=Guid.Parse( "E23D5024-78B7-465E-95E7-F0509E41AA4E"), Name= "Vendida" },
        };

        public static ListDataInfo<ListPendingTempResponse> GetListPendingResponse => new ListDataInfo<ListPendingTempResponse>
        {
            Count = 2,
            Data = new List<ListPendingTempResponse> {
                 new ListPendingTempResponse
                 {
                      Days = 1,
                      OperationDate = DateTime.Now,
                      NamePayer="Nombre del pagador",
                      NameSaller="Nombre del vendedor",
                      Nro=1,
                      Offer=1,
                      PurchasePercentage=70,
                      Status="",
                      TxPurchased="2/4",
                      EndDate=DateTime.Now.AddDays(10),
                      ExpectedDate=DateTime.Now,
                 },
                 new ListPendingTempResponse
                 {
                      Days = 1,
                      OperationDate = DateTime.Now,
                      NamePayer="Nombre del pagador",
                      NameSaller="Nombre del vendedor",
                      Nro=1,
                      Offer=1,
                      PurchasePercentage=100,
                      Status="",
                      TxPurchased="2/4",
                      EndDate=DateTime.Now.AddDays(10),
                      ExpectedDate=DateTime.Now,
                 },
                  new ListPendingTempResponse
                 {
                      Days = 1,
                      OperationDate = DateTime.Now,
                      NamePayer="Nombre del pagador",
                      NameSaller="Nombre del vendedor",
                      Nro=1,
                      Offer=1,
                      PurchasePercentage=100,
                      Status="Habilitada",
                      TxPurchased="2/4",
                      EndDate=DateTime.Now.AddDays(10),
                      ExpectedDate=DateTime.Now,
                 }
             },
        };
    }
}