using yourInvoice.Link.Application.LinkingProcess.UpdateGeneralInformation;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GeneralInformation
    {
        public static UpdateGeneralInformationCommand UpdateGeneralInformationCommandCommandNoValid => new UpdateGeneralInformationCommand(
            new Domain.LinkingProcesses.GeneralInformations.GeneralInformation
            {
                Id = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                FirstName = "nombre",
                Address = "direccion",
                Email = "correo@yopmail.com",
                City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
                PhoneNumber = "3202155524"
            });

        public static UpdateGeneralInformationCommand UpdateGeneralInformationCommandCommandValid => new UpdateGeneralInformationCommand(
            new Domain.LinkingProcesses.GeneralInformations.GeneralInformation
            {
                Id = Guid.Empty,
                FirstName = "nombre",
                Address = "direccion",
                Email = "correo@yopmail.com",
                City = Guid.Parse("DC7BA045-5596-4001-A2DA-95D012086CFA"),
                Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
                DepartmentState = Guid.Parse("3CD2309E-034D-4B0F-8701-BB1838ECEE75"),
                PhoneNumber = "3202155524"
            });
    }
}
