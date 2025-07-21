///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.CreateLegalSignatureDeclaration;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSignatureDeclaration;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class LegalSignatureDeclarationData
    {
        public static CreateLegalSignatureDeclarationCommand CreateLegalSignatureDeclarationCommand =>
            new CreateLegalSignatureDeclarationCommand(
                new LegalSignatureDeclarationCommand
                {
                    Id_LegalGeneralInformation = Guid.Parse("CB4503C7-DE4D-4E33-BC1D-80DBFC497452"),
                    Completed = Guid.NewGuid(),
                    Statements = true,
                    CommitmentAcceptRiskManagement = true,
                    ResponsivilityForInformation = true,
                    VisitAuthorization = true,
                });

        public static CreateLegalSignatureDeclarationCommand CreateLegalSignatureDeclarationCommandEmpy =>
           new CreateLegalSignatureDeclarationCommand(
               new LegalSignatureDeclarationCommand());

        public static GetLegalSignatureDeclarationQuery GetLegalSignatureDeclarationQuery =>
            new GetLegalSignatureDeclarationQuery(Guid.Parse("CB4503C7-DE4D-4E33-BC1D-80DBFC497452"));

        public static GetLegalSignatureDeclarationQuery GetLegalSignatureDeclarationEmpty =>
        new GetLegalSignatureDeclarationQuery(Guid.Empty);

        public static GetAccounLegalGeneralResponse GetAccounLegalGeneralResponse =>
            new GetAccounLegalGeneralResponse
            {
                Nit = "1111",
                CheckDigit = "1",
                City = "ciudad",
                DocumentNumber = "22222",
            };

        public static List<string> GetParagraph = new List<string>() { "Parrafo1", "parrafo2", "parrafo3", "parrafo4" };

        public static LegalSignatureDeclaration LegalSignatureDeclaration =>
            new LegalSignatureDeclaration(
                   id: Guid.NewGuid(),
                   id_LegalGeneralInformation: Guid.NewGuid(),
                   commitmentAcceptRiskManagement: false,
                   responsivilityForInformation: false,
                   visitAuthorization: true,
                   statements: false,
                   completed: Guid.NewGuid(),
                   statusId: Guid.NewGuid(),
                   statusDate: DateTime.Now,
                   status: true,
                   modifiedOn: DateTime.Now,
                   modifiedBy: Guid.NewGuid(),
                   createdOn: DateTime.Now,
                   createdBy: Guid.NewGuid()

                );
    }
}