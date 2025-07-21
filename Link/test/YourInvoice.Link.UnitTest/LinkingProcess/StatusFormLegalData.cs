///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class StatusFormLegalData
    {
        public static LegalGeneralInformation LegalGeneralInformation => new LegalGeneralInformation(
              id: Guid.NewGuid(),
              nit: "111111",
              checkDigit: "2",
              companyName: "companyName",
              companyTypeId: Guid.NewGuid(),
              societyTypeId: Guid.NewGuid(),
              societyTypeDetail: "detalle sociedad",
              economicActivityId: Guid.NewGuid(),
              economicActivityDetail: "actividad economica",
              CiiuCode: Guid.NewGuid(),
              greatContributorId: Guid.NewGuid(),
              isSelfRetaining: Guid.NewGuid(),
              fee: 100,
              originResources: "origen recursos",
              emailCorporate: "emailcorporate@hotmail.com",
              electronicInvoiceEmail: "electronicinvoice@hotmail.com",
              phoneNumber: "222222",
              countryId: Guid.NewGuid(),
              departmentId: Guid.NewGuid(),
              cityId: Guid.NewGuid(),
              address: "direccion",
              branchAddress: "direccion sucursal",
              branchPhoneNumber: "33333333",
              branchDepartmentId: Guid.NewGuid(),
              branchCityId: Guid.NewGuid(),
              branchContactName: "contact name",
              branchDocumentNumberTypeId: Guid.NewGuid(),
              branchDocumentNumber: "44444",
              branchContactPhone: "555555",
              branchEmailContact: "emailcontact@hotmail.com",
              branchPosition: "puesto trabajo",
              completed: Guid.NewGuid(),
              statusId: Guid.Empty,
              statusDate: DateTime.Now,
              createdOn: DateTime.Now

            );

        public static LegalBoardDirector LegalBoardDirector => new LegalBoardDirector(
            id: Guid.NewGuid(),
            id_LegalGeneralInformation: Guid.NewGuid(),
            fullNameCompanyName: "nombre compañia",
            documentTypeId: Guid.NewGuid(),
            documentNumber: "111111",
            phoneNumber: "222222",
            completed: Guid.NewGuid(),
            statusId: Guid.Empty,
            statusDate: DateTime.Now,
            status: true,
            createOn: DateTime.Now,
            createBy: Guid.NewGuid(),
            modifiedOn: DateTime.Now,
            modifiedBy: Guid.NewGuid()
           );

        public static LegalFinancialInformation LegalFinancialInformation => new LegalFinancialInformation(
             id: Guid.NewGuid(),
             idLegalGeneralInformation: Guid.NewGuid(),
             totalAssets: 10000,
             totalLiabilities: 20000,
             totalMonthlyIncome: 30000,
             monthlyIncome: 400000,
             monthlyExpenditures: 500000,
             otherIncome: 60000,
             describeOriginIncome: "descripcion",
             operationsForeignCurrency: Guid.NewGuid(),
             operationsType: "",
             operationTypeDetail: "detalle operacion",
             accountsForeignCurrency: Guid.NewGuid(),
             accountNumber: "12212121",
             bank: "nombre banco",
             amount: 909090,
             city: "ciudad",
             currency: "3434343",
             completed: Guid.NewGuid(),
             createdOn: DateTime.Now,
             statusId: Guid.NewGuid(),
             statusDate: DateTime.Now
            );

        public static LegalRepresentativeTaxAuditor LegalRepresentativeTaxAuditor => new LegalRepresentativeTaxAuditor(
            id: Guid.NewGuid(),
            id_LegalGeneralInformation: Guid.NewGuid(),
            firstName: string.Empty,
            secondName: "",
            lastName: "",
            secondLastName: "",
            documentTypeId: Guid.NewGuid(),
            documentNumber: "11111",
            expeditionDate: DateTime.Now,
            expeditionCountry: "pais expedicion",
            email: "email@pruebas.com",
            homeAddress: "",
            phone: "3103045667",
            departmentState: Guid.NewGuid(),
            city: Guid.NewGuid(),
            taxAuditorFirstName: "",
            taxAuditorSecondName: "",
            taxAuditorLastName: "",
            taxAuditorSecondLastName: "",
            taxAuditorDocumentTypeId: Guid.NewGuid(),
            taxAuditorDocumentNumber: "3454545",
            taxAuditorPhoneNumber: "3008909090",
            completed: Guid.NewGuid(),
            statusId: Guid.NewGuid(),
            statusDate: DateTime.Now,
            createdOn: DateTime.Now
            );

        public static LegalShareholder LegalShareholder => new LegalShareholder();
        public static LegalCommercialAndBankReference LegalCommercialAndBankReference => new LegalCommercialAndBankReference();

        public static LegalSAGRILAFT LegalSAGRILAFT => new LegalSAGRILAFT(
            id: Guid.NewGuid(),
            id_LegalGeneralInformation: Guid.NewGuid(),
            questionIdentifier: Guid.NewGuid(),
            responseIdentifier: Guid.NewGuid(),
            responseDetail: "detalle",
            completed: Guid.NewGuid(),
            statusId: Guid.NewGuid(),
            statusDate: DateTime.Now
            );
    }
}