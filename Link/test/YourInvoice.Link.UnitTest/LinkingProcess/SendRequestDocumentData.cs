///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.Accounts.Queries;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class SendRequestDocumentData
    {
        public static AccountResponse GetRequestDocumentResponse => new AccountResponse
        {
            Email = "email@pruebas.com",
            StatusDate = DateTime.Now,
            Name = "Name",
            Date = string.Empty,

        };

        public static AccountResponse GetRequestDocumentResponseEmpty => null;        
    }
}