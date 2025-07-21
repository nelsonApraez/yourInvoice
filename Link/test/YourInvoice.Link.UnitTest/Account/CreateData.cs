///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Application.Accounts.CreateAccount;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;

namespace yourInvoice.Link.UnitTest.Account
{
    public static class CreateData
    {
        public static CatalogItemInfo EmailPort => new CatalogItemInfo() { Name = "EMAIL Port", Descripton = "587" };

        public static CatalogItemInfo EmailServer => new CatalogItemInfo { Name = "EMAIL Server", Descripton = "smtp.gmail.com" };

        public static CatalogItemInfo EmailUser => new CatalogItemInfo { Name = "EMAIL User", Descripton = "yourInvoicenotification@gmail.com" };

        public static CatalogItemInfo EmailPasProjrd => new CatalogItemInfo { Name = "EMAIL PasProjrd", Descripton = "ghkohgjmxfepfnbb" };

        public static CatalogItemInfo EmailFrom => new CatalogItemInfo { Name = "EMAIL From", Descripton = "yourInvoicenotification@gmail.com" };

        public static CatalogItemInfo EmailSender => new CatalogItemInfo { Name = "EMAIL Sender", Descripton = "yourInvoicenotification@gmail.com" };

        public static CreateAccountCommand CreateAccountRequest => new(
            Guid.NewGuid(), 
            "1", 
            "2", 
            "r", 
            "n", 
            "sc", 
            "ln", 
            "sln", 
            Guid.NewGuid(), 
            "1", 
            "test@test.com", 
            "1", 
            Guid.NewGuid(), 
            "1", 
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            Guid.NewGuid()
        );

        public static CreateAccountCommand CreateAccountRequestNameEmpty => new(
            Guid.NewGuid(),
            "1",
            "2",
            "r",
            null,
            "sc",
            "ln",
            "sln",
            Guid.NewGuid(),
            "1",
            "test@test.com",
            "1",
            Guid.NewGuid(),
            "1",
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        public static Domain.Accounts.Account AccountCreated => new Domain.Accounts.Account
        {
            Id = Guid.NewGuid(),
            PersonTypeId = Guid.Empty,
            Nit = "",
            DigitVerify = "",
            SocialReason = "",
            Name = "",
            SecondName = "",
            LastName = "",
            SecondLastName = "",
            DocumentTypeId = Guid.Empty,
            DocumentNumber = "",
            Email = "",
            MobileCountryId = Guid.Empty,
            MobileNumber = "",
            PhoneCountryId = Guid.Empty,
            PhoneNumber = "",
            ContactById = Guid.Empty,
            Description = "",
            StatusId = Guid.Empty,
            StatusDate = DateTime.Now,
            Time = 0,
            CreatedOn = DateTime.Now,
            CreatedBy = Guid.Empty
        };

        public static GeneralInformation GeneralInformationCreated => new GeneralInformation
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
    }
}
