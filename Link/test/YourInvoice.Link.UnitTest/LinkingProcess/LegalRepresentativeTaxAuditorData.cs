///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.CreateLegalRepresentativeTaxAuditor;
using yourInvoice.Link.Application.LinkingProcess.GetLegalRepresentativeTaxAuditor;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class LegalRepresentativeTaxAuditorData
    {
        public static CreateLegalRepresentativeTaxAuditorCommand CreateLegalRepresentativeTaxAuditorCommand =>
            new CreateLegalRepresentativeTaxAuditorCommand(
                new LegalRepresentativeTax
                {
                    Id_LegalGeneralInformation = Guid.Parse("CB4503C7-DE4D-4E33-BC1D-80DBFC497452"),
                    City = Guid.NewGuid(),
                });

        public static CreateLegalRepresentativeTaxAuditorCommand CreateLegalRepresentativeTaxAuditorCommandEmpy =>
           new CreateLegalRepresentativeTaxAuditorCommand(
               new LegalRepresentativeTax());

        public static GetLegalRepresentativeTaxAuditorQuery GetLegalRepresentativeTaxAuditorQuery =>
            new GetLegalRepresentativeTaxAuditorQuery(Guid.Parse("CB4503C7-DE4D-4E33-BC1D-80DBFC497452"));

        public static GetLegalRepresentativeTaxAuditorQuery GetLegalRepresentativeTaxAuditorQueryEmpty =>
        new GetLegalRepresentativeTaxAuditorQuery(Guid.Empty);

        public static LegalRepresentativeTaxAuditor LegalRepresentativeTaxAuditor =>
            new LegalRepresentativeTaxAuditor(
               id: Guid.NewGuid(),
               id_LegalGeneralInformation: Guid.NewGuid(),
               firstName: "primer nombre",
               secondName: "segundo nombre",
                lastName: "last name",
                secondLastName: "secondlastname",
                documentTypeId: Guid.NewGuid(),
                documentNumber: "11111",
                expeditionDate: DateTime.Now,
                expeditionCountry: "pais de expedicion",
                email: "email@prueba.com",
                homeAddress: "direccion casa",
                phone: "3003202323",
                departmentState: Guid.NewGuid(),
                city: Guid.NewGuid(),
                taxAuditorFirstName: "taxAuditorFirstName",
                taxAuditorSecondName: "taxAuditorSecondName",
                taxAuditorLastName: "taxAuditorLastName",
                taxAuditorSecondLastName: "taxASecondLastName",
                taxAuditorDocumentTypeId: Guid.NewGuid(),
                taxAuditorDocumentNumber: "222222222",
                taxAuditorPhoneNumber: "333333333333",
                completed: Guid.NewGuid(),
                statusId: Guid.NewGuid(),
                statusDate: DateTime.Now,
                createdOn: DateTime.Now
                );
    }
}