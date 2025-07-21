///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Link.UnitTest.Account
{
    public static class ValidityData
    {
        public static GetRoleResponse GetRoleResNull => new GetRoleResponse();

        public static GetRoleResponse GetRoleRes => new GetRoleResponse
        {
            RoleId = Guid.Parse("72CA7D80-DE02-49A6-AB26-588CFE2A8F5B"),
            RoleName = "Administrador",
            Name = "prueba",
            Email = "prueba@prueba.com"
        };

        public static GetRoleResponse GetRoleResponseEmpty = null;

        public static Domain.Accounts.Account GetAccountResponse => new Domain.Accounts.Account
        {
            Id = Guid.NewGuid(),
            PersonTypeId = Guid.Parse("C77E3528-E582-435D-A527-700C5B57F9CA"),
            Nit = "2222222",
            DigitVerify = "4",
            SocialReason = "SocialReason",
            Name = "Name",
            SecondName = "SecondName",
            LastName = "LastName",
            SecondLastName = "SecondLastName",
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
            CreatedBy = Guid.Empty,
        };

        public static Domain.Accounts.Account GetAccountNaturalResponse => new Domain.Accounts.Account
        {
            Id = Guid.NewGuid(),
            PersonTypeId = Guid.Parse("5C2F2B7E-EDDA-4E5F-B875-08072E206A83"),
            Nit = "2222222",
            DigitVerify = "4",
            SocialReason = "SocialReason",
            Name = "Name",
            SecondName = "SecondName",
            LastName = "LastName",
            SecondLastName = "SecondLastName",
            DocumentTypeId = Guid.Empty,
            DocumentNumber = "",
            Email = "prueba@hotmail.com",
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
            CreatedBy = Guid.Empty,
        };

        public static Domain.Accounts.Account GetAccountApprovedResponse => new Domain.Accounts.Account
        {
            Id = Guid.NewGuid(),
            PersonTypeId = Guid.Parse("C77E3528-E582-435D-A527-700C5B57F9CA"),
            Nit = "111111",
            DigitVerify = "2",
            SocialReason = "Social",
            Name = "Name",
            SecondName = "SecondName ",
            LastName = "LastName",
            SecondLastName = "SecondLastName",
            DocumentTypeId = Guid.Empty,
            DocumentNumber = "",
            Email = "",
            MobileCountryId = Guid.Empty,
            MobileNumber = "",
            PhoneCountryId = Guid.Empty,
            PhoneNumber = "",
            ContactById = Guid.Empty,
            Description = "",
            StatusId = Guid.Parse("0306CE79-BF38-43F1-A923-30815EADFE79"),
            StatusDate = DateTime.Now,
            Time = 0,
            CreatedOn = DateTime.Now,
            CreatedBy = Guid.Empty,
        };

        public static Domain.Accounts.Account GetAccountResponseEmpty = null;

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

        public static GeneralInformation GetGeneralInformationApprovedResponse => new GeneralInformation
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
            StatusId = Guid.Parse("0306CE79-BF38-43F1-A923-30815EADFE79"),
            StatusDate = DateTime.Now,
            CreatedOn = DateTime.Now,
            CreatedBy = Guid.Empty
        };

        public static GeneralInformation GetGeneralInformationResponseEmpty = null;

        public static GetRoleResponse GetRoleResponse => new GetRoleResponse
        {
            Email = "email@hotmail.com",
            Name = "Name",
            RoleId = Guid.NewGuid(),
            RoleName = "Nombre del rol",
        };
    }
}