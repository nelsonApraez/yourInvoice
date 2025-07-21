///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Domain.Accounts.Queries;

namespace yourInvoice.Link.UnitTest.Account
{
    public static class ListData
    {
        public static SearchInfo GetSearchInfo => new SearchInfo
        {
            ColumnOrder = "Name",
            PageSize = 0,
            OrderType = "asc",
        };

        public static SearchInfo GetSearchInfoOrder => new SearchInfo
        {
            ColumnOrder = "CreatedOn",
            OrderType = "desc",            
            PageSize = 0,
        };

        public static ListDataInfo<ListResponse> GetListResponse => new ListDataInfo<ListResponse>
        {
            Count = 2,
            Data = new List<ListResponse> {
                new ListResponse
                 {
                      Id=Guid.NewGuid(),
                      Name="Name",
                      DocumentNumber="1234567",
                      DocumentType="cedula",
                      Email="email@gmail.com",
                      PhoneNumber="3102344556",
                      Status="Pendiente",
                      UserType="Vendedor",
                      OrderRegister=1,
                      CreatedOn=DateTime.Now.AddDays(-1),
                      Time=2,
                 },
                 new ListResponse
                 {
                    Id=Guid.NewGuid(),
                    Name="Name2",
                    DocumentNumber="123456766",
                    DocumentType="cedula1",
                    Email="email2@gmail.com",
                    PhoneNumber="31023445562",
                    Status="Pendiente",
                    UserType="Comprador",
                    CreatedOn=DateTime.Now.AddDays(-1),
                    OrderRegister=1,
                    Time=2,
                 },
                 new ListResponse
                 {
                      Id=Guid.NewGuid(),
                      Name="Name",
                      DocumentNumber="1234567",
                      DocumentType="cedula",
                      Email="email@gmail.com",
                      PhoneNumber="3102344556",
                      Status="Rechazado",
                      UserType="Vendedor",
                      OrderRegister=3,
                      CreatedOn=DateTime.Now.AddDays(-1),
                      Time=2,
                 },
                 new ListResponse
                 {
                    Id=Guid.NewGuid(),
                    Name="Name2",
                    DocumentNumber="123456766",
                    DocumentType="cedula1",
                    Email="email2@gmail.com",
                    PhoneNumber="31023445562",
                    Status="Rechazado",
                    UserType="Comprador",
                    CreatedOn=DateTime.Now.AddDays(-1),
                    OrderRegister=3,
                    Time=2,
                 }
             },
        };
    }
}