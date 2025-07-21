
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
using yourInvoice.Link.Application.LinkingProcess.CreateLegalSAGRILAFT;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFT;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFTQuestionResponse;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class SagrilaftData
    {
        public static User GetUser => new User(
           Guid.Parse("9bd5a44f-2fdd-488e-9b76-878bce147fdc"), 2, "890104521", "test name", Guid.NewGuid(), "72335847",
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali",
                Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali",
                true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()
            );

        public static User GetUserNotValid => new User(
           Guid.Empty, 2, "", "", Guid.NewGuid(), "", "", "", "", "", "", "",
                Guid.NewGuid(), Guid.NewGuid(), "", "", "", "", "", "",
                true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()
            );

        public static CreateSagrilaftCommand GetCreateSagrilaftCommandValid => new CreateSagrilaftCommand(new List<Sagrilaft>()
        {
          new Sagrilaft() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("45C7A971-2176-4A7E-99D6-4B97768402AF"), ResponseIdentifier=Guid.Parse("90CCE2A5-EA2B-4CDF-B4C5-2A99A612256E"), ResponseDetail="" },
          new Sagrilaft() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("9F72A1B9-A178-482A-878F-7A39EB116291"), ResponseIdentifier=Guid.Parse("D86153EA-658B-4BC6-9A70-22D235B62478"), ResponseDetail="" },
          new Sagrilaft() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("11CEEE51-02F9-4F03-A963-7DC877DD8F33"), ResponseIdentifier=Guid.Parse("929F38F5-157D-464E-BE80-65000ECBE993"), ResponseDetail="" },
          new Sagrilaft() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("C2FD3F9B-9090-412A-8BD7-764CDCECA370"), ResponseIdentifier=Guid.Parse("525D0F2A-03D3-4169-BC46-B586C996BF35"), ResponseDetail="" },
          new Sagrilaft() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("75722C29-7560-4A15-96AA-C72E589EFA3B"), ResponseIdentifier=Guid.Parse("8DE55BB5-A711-4D7E-A8DD-5E047D8D1EC2"), ResponseDetail="" },
        });

        public static CreateSagrilaftCommand GetCreateSagrilaftCommand => new CreateSagrilaftCommand(new List<Sagrilaft>()
        {
          new Sagrilaft() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("45C7A971-2176-4A7E-99D6-4B97768402AF"), ResponseIdentifier=Guid.Parse("90CCE2A5-EA2B-4CDF-B4C5-2A99A612256E"), ResponseDetail="" },
          new Sagrilaft() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("9F72A1B9-A178-482A-878F-7A39EB116291"), ResponseIdentifier=Guid.Parse("D86153EA-658B-4BC6-9A70-22D235B62478"), ResponseDetail="" },
          new Sagrilaft() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("11CEEE51-02F9-4F03-A963-7DC877DD8F33"), ResponseIdentifier=Guid.Parse("929F38F5-157D-464E-BE80-65000ECBE993"), ResponseDetail="" },
          new Sagrilaft() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("C2FD3F9B-9090-412A-8BD7-764CDCECA370"), ResponseIdentifier=Guid.Parse("525D0F2A-03D3-4169-BC46-B586C996BF35"), ResponseDetail="" },
          new Sagrilaft() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("75722C29-7560-4A15-96AA-C72E589EFA3B"), ResponseIdentifier=Guid.Parse("8DE55BB5-A711-4D7E-A8DD-5E047D8D1EC2"), ResponseDetail="" },
         });

        public static UpdateSagrilaftCommand GetUpdateSagrilaftCommand => new UpdateSagrilaftCommand(new List<UpdateSagrilaft>()
        {
          new UpdateSagrilaft() { Id=Guid.Parse("82e5f542-b3a2-4b2c-8e03-056570559d5f"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"), Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("ea67d16c-ba6e-41cb-aebd-3c141b82dc9b"), ResponseIdentifier = Guid.Parse("816a9147-b439-4e16-9977-f7eeb3c44886"), ResponseDetail = "" },
          new UpdateSagrilaft() { Id=Guid.Parse("a43633f9-ec20-4ca3-ac59-1bb5729ff5a5"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("8693ac3d-dc03-4963-9caf-4ec30854b27d"), ResponseIdentifier = Guid.Parse("9f644040-5e85-4e37-8661-57c049f9a9c0"), ResponseDetail = "" },
          new UpdateSagrilaft() { Id=Guid.Parse("d97aa686-ee94-430e-8a9f-604da5dd17c3"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("a582d141-4ac3-4729-93d9-70f69e2a0257"), ResponseIdentifier = Guid.Parse("75254bc6-5e02-42b2-9c09-8e7eead16cd4"), ResponseDetail = "" },
          new UpdateSagrilaft() { Id=Guid.Parse("b2b12290-254e-4f12-b478-672b4316e7c2"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("158d8d7f-cdbb-4ec5-b5c8-7cd2c7398ad1"), ResponseIdentifier = Guid.Parse("7b0b625b-7994-446f-aeeb-d1831b558aa9"), ResponseDetail = "" },
          new UpdateSagrilaft() { Id=Guid.Parse("cbcd00ad-9890-43c2-b4aa-74d3c866f2d0"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("cdafb83b-db6a-4a02-a78c-e1d37fe32465"), ResponseIdentifier = Guid.Parse("512d2651-18e7-45d6-84bb-62488d34c447"), ResponseDetail = "" },
           });

        public static GetSagrilaftQuery GetGetSagrilaftQueryValid => new GetSagrilaftQuery(idLegalGeneralInformation: Guid.Parse("82e5f542-b3a2-4b2c-8e03-056570559d5f"));

        public static GetSagrilaftQuery GetGetSagrilaftQuery => new GetSagrilaftQuery(idLegalGeneralInformation: Guid.Empty);

        public static GetSagrilaftResponse GetSagrilaftResponse => new GetSagrilaftResponse()
        {
            Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
            CompletedDescription = "Completado",
            CompletedName = "Completado=Verde",
            SagrilaftAnswers = new List<GetSagrilaft> {
                new GetSagrilaft() {  Id=Guid.Parse("82e5f542-b3a2-4b2c-8e03-056570559d5f"), Id_LegaleneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),  QuestionIdentifier = Guid.Parse("ea67d16c-ba6e-41cb-aebd-3c141b82dc9b"), ResponseIdentifier = Guid.Parse("816a9147-b439-4e16-9977-f7eeb3c44886"), ResponseDetail = "" },
                new GetSagrilaft() { Id=Guid.Parse("a43633f9-ec20-4ca3-ac59-1bb5729ff5a5"), Id_LegaleneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"), QuestionIdentifier = Guid.Parse("8693ac3d-dc03-4963-9caf-4ec30854b27d"), ResponseIdentifier = Guid.Parse("9f644040-5e85-4e37-8661-57c049f9a9c0"), ResponseDetail = "" },
                new GetSagrilaft() { Id=Guid.Parse("d97aa686-ee94-430e-8a9f-604da5dd17c3"), Id_LegaleneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),QuestionIdentifier = Guid.Parse("a582d141-4ac3-4729-93d9-70f69e2a0257"), ResponseIdentifier = Guid.Parse("75254bc6-5e02-42b2-9c09-8e7eead16cd4"), ResponseDetail = "" },
                new GetSagrilaft() { Id=Guid.Parse("b2b12290-254e-4f12-b478-672b4316e7c2"), Id_LegaleneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"), QuestionIdentifier = Guid.Parse("158d8d7f-cdbb-4ec5-b5c8-7cd2c7398ad1"), ResponseIdentifier = Guid.Parse("7b0b625b-7994-446f-aeeb-d1831b558aa9"), ResponseDetail = "" },
                new GetSagrilaft() { Id=Guid.Parse("cbcd00ad-9890-43c2-b4aa-74d3c866f2d0"), Id_LegaleneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"), QuestionIdentifier = Guid.Parse("cdafb83b-db6a-4a02-a78c-e1d37fe32465"), ResponseIdentifier = Guid.Parse("512d2651-18e7-45d6-84bb-62488d34c447"), ResponseDetail = "" },
                }
        };

        public static GetSagrilaftQuestionAnswerQuery GetSagrilaftQuestionAnswerQuery = new GetSagrilaftQuestionAnswerQuery();

        public static List<GetSagrilaftQuestionsAnswerResponse> GetSagrilaftQuestionsAnswerResponse => new List<GetSagrilaftQuestionsAnswerResponse>()
        {
          new GetSagrilaftQuestionsAnswerResponse() {
                IdQuestion=Guid.Parse("9F72A1B9-A178-482A-878F-7A39EB116291"),
                DecriptionQuestion="¿Alguno de los administradores, accionistas o socios de la empresa, son personas expuestas politicamente (PEP) Nacionales, extranjeras o de organizaciones internacionales?",
                Detalle=false,
                 Answers= new List<GetSagrilaftAnswer>
                 {
                      new GetSagrilaftAnswer{ IdAnswer=Guid.Parse("C19C21AE-0D3C-4ED7-833E-6E433414E693"), DecriptionAnswer="NO" },
                      new GetSagrilaftAnswer{ IdAnswer=Guid.Parse("D86153EA-658B-4BC6-9A70-22D235B62478"), DecriptionAnswer="SI" }
                 }
          },
          new GetSagrilaftQuestionsAnswerResponse() {
                IdQuestion=Guid.Parse("45C7A971-2176-4A7E-99D6-4B97768402AF"),
                DecriptionQuestion="¿Cuenta la empresa con políticas y procedimientos para la prevención de los riesgos de Lavado de Activos/Financiación de Terrorismo?",
                Detalle=false,
                 Answers= new List<GetSagrilaftAnswer>
                 {
                      new GetSagrilaftAnswer{ IdAnswer=Guid.Parse("9977CBFA-F959-4DE6-99D8-3F952073EEF1"), DecriptionAnswer="NO" },
                      new GetSagrilaftAnswer{ IdAnswer=Guid.Parse("90CCE2A5-EA2B-4CDF-B4C5-2A99A612256E"), DecriptionAnswer="SI" }
                 }
          },
        };
    }
}