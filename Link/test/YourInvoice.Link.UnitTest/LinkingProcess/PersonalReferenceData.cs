///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.CreatePersonalReference;
using yourInvoice.Link.Application.LinkingProcess.GetPersonalReference;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class PersonalReferenceData
    {
        public static CreatePersonalReferenceCommand PersonalReferenceCommandCreate = new(
            Guid.NewGuid(),
            "prueba",
            "0000000",
            "prueba",
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        public static CreatePersonalReferenceCommand PersonalReferenceCommandCreateEmpty = null;

        public static PersonalReferences PersonalReferencesCreated => new PersonalReferences
        {
            Id = Guid.NewGuid(),
            Id_GeneralInformation = Guid.NewGuid(),
            NamePersonalReference = "prueba",
            PhoneNumber = "000000",
            NameBussines = "prueba",
            DepartmentState = Guid.NewGuid(),
            City = Guid.NewGuid(),
            Completed = Guid.NewGuid(),
            StatusId = Guid.NewGuid(),
            StatusDate = DateTime.Now,
            Status = false,
            CreatedBy = Guid.NewGuid(),
            CreatedOn = DateTime.Now
        };

        public static GetPersonalReferenceQuery GetPersonalReferenceRequest = new(Guid.NewGuid());

        public static GetReferenceResponse GetPersonalReferenceResponse => new GetReferenceResponse
        {
            Id = Guid.NewGuid(),
            Id_GeneralInformation = Guid.NewGuid(),
            NamePersonalReference = "prueba",
            PhoneNumber = "000000",
            NameBussines = "prueba",
            DepartmentState = Guid.NewGuid(),
            City = Guid.NewGuid(),
            Completed = Guid.NewGuid(),
            StatusId = Guid.NewGuid(),
            StatusDate = DateTime.Now,
        };
    }
}
