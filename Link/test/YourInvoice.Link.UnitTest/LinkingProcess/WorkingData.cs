///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.CreateWorking;
using yourInvoice.Link.Application.LinkingProcess.GetWorking;
using yourInvoice.Link.Application.LinkingProcess.UpdateWorking;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class WorkingData
    {
        public static CreateWorkingCommand CreateWorkingCommandNoValid => new CreateWorkingCommand(
            new Working
            {
                Id_GeneralInformation = Guid.Empty,
                Address = "direccion",
                BusinessName = "lugar de trabajo",
                City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
                PhoneNumber = 123456789012345,
                Position = "mensajero",
                Profession = "profesion",
            });

        public static CreateWorkingCommand CreateWorkingCommand => new CreateWorkingCommand(
             new Working
             {
                 Id_GeneralInformation = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 Address = "direccion",
                 BusinessName = "lugar de trabajo",
                 City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                 DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
                 PhoneNumber = 123456789012345,
                 Position = "mensajero",
                 Profession = "profesion",
             });

        public static UpdateWorkingCommand UpdateWorkingCommand => new UpdateWorkingCommand(
             new UpdateWorking
             {
                 Id = Guid.Parse("4f418af8-6358-4157-bfb2-3af6fa7993a9"),
                 Id_GeneralInformation = Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),
                 Address = "direccion",
                 BusinessName = "lugar de trabajo",
                 City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                 Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                 DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
                 PhoneNumber = 123456789012345,
                 Position = "mensajero",
                 Profession = "profesion",
             }
            );

        public static GetWorkingResponse GetWorkingResponse => new GetWorkingResponse()
        {
            Id = Guid.Parse("4f418af8-6358-4157-bfb2-3af6fa7993a9"),
            Id_GeneralInformation = Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),
            Address = "direccion",
            BusinessName = "lugar de trabajo",
            City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
            Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
            DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
            PhoneNumber = 123456789012345,
            Position = "mensajero",
            Profession = "profesion",
            DescriptionCity = "Medellin",
            DescriptionCompleted = "Aprobado",
            DescriptionDepartmentState = "Antioquia"
        };

        public static GetWorkingQuery GetWorkingQueryValid => new GetWorkingQuery(Id_GeneralInformation: Guid.NewGuid());
        public static GetWorkingQuery GetWorkingQuery => new GetWorkingQuery(Id_GeneralInformation: Guid.Empty);
    }
}