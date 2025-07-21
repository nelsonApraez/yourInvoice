///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.CreateLegalGeneralInformation;
using yourInvoice.Link.Application.LinkingProcess.GetLegalGeneralInformation;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class LegalGeneralInformationData
    {
        public static CreateLegalGeneralInformationCommand CreateLegalGeneralInformationCommand
            => new CreateLegalGeneralInformationCommand(
                new LegalGeneral
                {
                    Id = Guid.Parse("CB4503C7-DE4D-4E33-BC1D-80DBFC497452"),
                    Nit = "0000000",
                    CompanyName = "CompanyName",
                    CheckDigit = "9",
                });

        public static CreateLegalGeneralInformationCommand CreateLegalGeneralInformationCommandEmpy
           => new CreateLegalGeneralInformationCommand(
               new LegalGeneral
               {
                   Id = Guid.Empty,
                   Nit = null,
                   CompanyName = string.Empty,
                   CheckDigit = string.Empty,
               });

        public static GetLegalGeneralInformationResponse GetLegalGeneralInformationResponse
           => new GetLegalGeneralInformationResponse
           {
               Id = Guid.Parse("CB4503C7-DE4D-4E33-BC1D-80DBFC497452"),
               Address = "Address",
               BranchAddress = "BranchAddress",
           };

        public static GetLegalGeneralInformationQuery GetLegalGeneralInformationQuery =>
            new GetLegalGeneralInformationQuery(
                Id: Guid.Parse("CB4503C7-DE4D-4E33-BC1D-80DBFC497452")
                );

        public static GetLegalGeneralInformationQuery GetLegalGeneralInformationQueryEmpy =>
           new GetLegalGeneralInformationQuery(
               Id: Guid.Empty
               );
    }
}