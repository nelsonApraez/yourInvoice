///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using G = yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class StatusFormData
    {
        public static ExposureInformation GetExposureInformation => new ExposureInformation(
            id: Guid.NewGuid()
            , id_GeneralInformation: Guid.NewGuid()
            , questionIdentifier: Guid.NewGuid()
            , responseIdentifier: Guid.NewGuid()
            , responseDetail: "detalle"
            , completed: Guid.NewGuid()
            , statusId: Guid.NewGuid()
            , statusDate: DateTime.Now
            , declarationOriginFunds: string.Empty
            );

        public static BankInformation GetBankInformation => new BankInformation(
            id: Guid.NewGuid()
            , id_GeneralInformation: Guid.NewGuid()
            , bankReference: Guid.NewGuid()
            , phoneNumber: "310333456"
            , bankProduct: "producto banco"
            , departmentState: Guid.NewGuid()
            , city: Guid.NewGuid()
            , completed: Guid.NewGuid()
            , createdOn: DateTime.Now
            , statusId: Guid.NewGuid()
            , statusDate: DateTime.Now
            );

        public static FinancialInformation GetFinancialInformation => new FinancialInformation(
             Guid.NewGuid()
            , idGeneralInformation: Guid.NewGuid()
           , totalAssets: 10000
            , totalLiabilities: 20000
            , totalWorth: 300000
            , monthlyIncome: 10000
            , monthlyExpenditures: 40000
            , otherIncome: 50000
            , describeOriginIncome: "descripcion origen"
           , completed: Guid.NewGuid()
            , createdOn: DateTime.Now
           , statusId: Guid.NewGuid()
           , statusDate: DateTime.Now
           );

        public static G.GeneralInformation GetGeneralInformation => new G.GeneralInformation(
          Guid.NewGuid()
            , firstName: "primer nombre"
            , secondName: "segundo nombre"
            , lastName: "primer apellido"
            , secondLastName: "segundo apellido"
            , documentTypeId: Guid.NewGuid()
            , documentNumber: "23456778"
            , expeditionDate: DateTime.Now
            , expeditionCountry: Guid.NewGuid()
            , economicActivity: Guid.NewGuid()
            , secondaryEconomicActivity: Guid.NewGuid()
            , email: "correo@prueba.com"
            , phoneNumber: "3225676767"
            , movilPhoneNumber: "3105889900"
            , departmentState: Guid.NewGuid()
            , city: Guid.NewGuid()
            , address: "direccion"
            , phoneCorrespondence: "correspondencia"
            , departmentStateCorrespondence: Guid.NewGuid()
            , cityCorrespondence: Guid.NewGuid()
            , addressCorrespondence: "direccion correspondencia"
            , completed: Guid.NewGuid()
            , linkStatus: Guid.NewGuid()
            , statusId: Guid.NewGuid()
            , statusDate: DateTime.Now
            , createdOn: DateTime.Now
            );

        public static PersonalReferences GetPersonalReferences => new PersonalReferences(
             Guid.NewGuid()
            , id_GeneralInformation: Guid.NewGuid()
            , namePersonalReference: "nombre referencia personal"
            , phoneNumber: "1234566"
            , nameBussines: "negocio"
            , departmentState: Guid.NewGuid()
            , city: Guid.NewGuid()
            , completed: Guid.NewGuid()
            , statusId: Guid.NewGuid()
            , statusDate: DateTime.Now
            , status: true
            , createOn: DateTime.Now
            , createBy: Guid.NewGuid()
            , modifiedOn: DateTime.Now
            , modifiedBy: Guid.NewGuid()
            );

        public static WorkingInformation GetWorkingInformation => new WorkingInformation(
             Guid.NewGuid()
            , id_GeneralInformation: Guid.NewGuid()
            , businessName: "nombre negocio"
            , profession: "profesion"
            , position: "posicion"
            , phoneNumber: 10101030
            , departmentState: Guid.NewGuid()
            , city: Guid.NewGuid()
            , address: "direccion"
            , completed: Guid.NewGuid()
            , statusId: Guid.NewGuid()
            , whatTypeProductServiceSell: "tipo producto servicio venta"
            , statusDate: DateTime.Now
            );
    }
}