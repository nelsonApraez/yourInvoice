///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.Accounts.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;

namespace yourInvoice.Link.UnitTest.Account
{
    public static class ApproveData
    {
        public static int SaveChange => 1;

        public static AccountResponse GetAccountResponse => new AccountResponse
        {
            Email = "email@pruebas.com",
            StatusDate = DateTime.Now,
            Name = "Name",
            Date = string.Empty,

        };

        public static AccountResponse GetAccountResponseEmpty => null;

        public static GeneralInformation GetGeneralInformationResponse => new GeneralInformation
        {
            Id = Guid.NewGuid(),
            FirstName = "pruebas",
            SecondName = "",
            LastName = "pruebas",
            SecondLastName = "",
            DocumentTypeId = Guid.Empty,
            DocumentNumber = "",
            ExpeditionDate = null,
            EconomicActivity = Guid.Empty,
            SecondaryEconomicActivity = Guid.Empty,
            Email = "pruebas@pruebas.com",
            PhoneNumber = "",
            MovilPhoneNumber = "",
            DepartmentState = Guid.Empty,
            City = Guid.Empty,
            Address = "",
            Completed = Guid.Empty,
            LinkStatus = Guid.Empty,
            StatusId = Guid.Empty,
            StatusDate = DateTime.Now,
            CreatedOn = DateTime.Now,
            CreatedBy = Guid.Empty
        };

        public static GeneralInformation GetGeneralInformationResponseEmpty => null;
    }
}