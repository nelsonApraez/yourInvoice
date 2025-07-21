///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.CreateExposure;
using yourInvoice.Link.Application.LinkingProcess.GetExposure;
using yourInvoice.Link.Application.LinkingProcess.GetExposureQuestionResponse;
using yourInvoice.Link.Application.LinkingProcess.UpdateExposure;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public static class ExposureData
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

        public static CreateExposureCommand GetCreateExposureCommandValid => new CreateExposureCommand(new List<Exposure>()
        {
          new Exposure() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("ea67d16c-ba6e-41cb-aebd-3c141b82dc9b"), ResponseIdentifier=Guid.Parse("816a9147-b439-4e16-9977-f7eeb3c44886"), ResponseDetail="" },
          new Exposure() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("8693ac3d-dc03-4963-9caf-4ec30854b27d"), ResponseIdentifier=Guid.Parse("9f644040-5e85-4e37-8661-57c049f9a9c0"), ResponseDetail="" },
          new Exposure() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("a582d141-4ac3-4729-93d9-70f69e2a0257"), ResponseIdentifier=Guid.Parse("75254bc6-5e02-42b2-9c09-8e7eead16cd4"), ResponseDetail="" },
          new Exposure() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("158d8d7f-cdbb-4ec5-b5c8-7cd2c7398ad1"), ResponseIdentifier=Guid.Parse("7b0b625b-7994-446f-aeeb-d1831b558aa9"), ResponseDetail="" },
          new Exposure() { Id_GeneralInformation=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("cdafb83b-db6a-4a02-a78c-e1d37fe32465"), ResponseIdentifier=Guid.Parse("512d2651-18e7-45d6-84bb-62488d34c447"), ResponseDetail="" },
        });

        public static CreateExposureCommand GetCreateExposureCommand => new CreateExposureCommand(new List<Exposure>()
        {
          new Exposure() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("ea67d16c-ba6e-41cb-aebd-3c141b82dc9b"), ResponseIdentifier=Guid.Parse("816a9147-b439-4e16-9977-f7eeb3c44886"), ResponseDetail="" },
          new Exposure() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("8693ac3d-dc03-4963-9caf-4ec30854b27d"), ResponseIdentifier=Guid.Parse("9f644040-5e85-4e37-8661-57c049f9a9c0"), ResponseDetail="" },
          new Exposure() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("a582d141-4ac3-4729-93d9-70f69e2a0257"), ResponseIdentifier=Guid.Parse("75254bc6-5e02-42b2-9c09-8e7eead16cd4"), ResponseDetail="" },
          new Exposure() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("158d8d7f-cdbb-4ec5-b5c8-7cd2c7398ad1"), ResponseIdentifier=Guid.Parse("7b0b625b-7994-446f-aeeb-d1831b558aa9"), ResponseDetail="" },
          new Exposure() { Completed=Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier=Guid.Parse("cdafb83b-db6a-4a02-a78c-e1d37fe32465"), ResponseIdentifier=Guid.Parse("512d2651-18e7-45d6-84bb-62488d34c447"), ResponseDetail="" },
        });

        public static UpdateExposureCommand GetUpdateExposureCommand => new UpdateExposureCommand(new List<UpdateExposure>()
        {
          new UpdateExposure() { Id=Guid.Parse("82e5f542-b3a2-4b2c-8e03-056570559d5f"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"), Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("ea67d16c-ba6e-41cb-aebd-3c141b82dc9b"), ResponseIdentifier = Guid.Parse("816a9147-b439-4e16-9977-f7eeb3c44886"), ResponseDetail = "" },
          new UpdateExposure() { Id=Guid.Parse("a43633f9-ec20-4ca3-ac59-1bb5729ff5a5"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("8693ac3d-dc03-4963-9caf-4ec30854b27d"), ResponseIdentifier = Guid.Parse("9f644040-5e85-4e37-8661-57c049f9a9c0"), ResponseDetail = "" },
          new UpdateExposure() { Id=Guid.Parse("d97aa686-ee94-430e-8a9f-604da5dd17c3"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("a582d141-4ac3-4729-93d9-70f69e2a0257"), ResponseIdentifier = Guid.Parse("75254bc6-5e02-42b2-9c09-8e7eead16cd4"), ResponseDetail = "" },
          new UpdateExposure() { Id=Guid.Parse("b2b12290-254e-4f12-b478-672b4316e7c2"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("158d8d7f-cdbb-4ec5-b5c8-7cd2c7398ad1"), ResponseIdentifier = Guid.Parse("7b0b625b-7994-446f-aeeb-d1831b558aa9"), ResponseDetail = "" },
          new UpdateExposure() { Id=Guid.Parse("cbcd00ad-9890-43c2-b4aa-74d3c866f2d0"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"), QuestionIdentifier = Guid.Parse("cdafb83b-db6a-4a02-a78c-e1d37fe32465"), ResponseIdentifier = Guid.Parse("512d2651-18e7-45d6-84bb-62488d34c447"), ResponseDetail = "" },
           });

        public static GetExposureQuery GetGetExposureQueryValid => new GetExposureQuery(idGeneralInformation: Guid.Parse("82e5f542-b3a2-4b2c-8e03-056570559d5f"));

        public static GetExposureQuery GetGetExposureQuery => new GetExposureQuery(idGeneralInformation: Guid.Empty);

        public static GetExposureResponse GetExposureResponse => new GetExposureResponse()
        {
            Completed = Guid.Parse("9a2a47f3-27d8-4a54-9b7d-75b63ea3e4a8"),
            CompletedDescription = "Completado",
            CompletedName = "Completado=Verde",
            ExposureAnswers = new List<GetExposure> {
                new GetExposure() {  Id=Guid.Parse("82e5f542-b3a2-4b2c-8e03-056570559d5f"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),  QuestionIdentifier = Guid.Parse("ea67d16c-ba6e-41cb-aebd-3c141b82dc9b"), ResponseIdentifier = Guid.Parse("816a9147-b439-4e16-9977-f7eeb3c44886"), ResponseDetail = "" },
                new GetExposure() { Id=Guid.Parse("a43633f9-ec20-4ca3-ac59-1bb5729ff5a5"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"), QuestionIdentifier = Guid.Parse("8693ac3d-dc03-4963-9caf-4ec30854b27d"), ResponseIdentifier = Guid.Parse("9f644040-5e85-4e37-8661-57c049f9a9c0"), ResponseDetail = "" },
                new GetExposure() { Id=Guid.Parse("d97aa686-ee94-430e-8a9f-604da5dd17c3"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"),QuestionIdentifier = Guid.Parse("a582d141-4ac3-4729-93d9-70f69e2a0257"), ResponseIdentifier = Guid.Parse("75254bc6-5e02-42b2-9c09-8e7eead16cd4"), ResponseDetail = "" },
                new GetExposure() { Id=Guid.Parse("b2b12290-254e-4f12-b478-672b4316e7c2"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"), QuestionIdentifier = Guid.Parse("158d8d7f-cdbb-4ec5-b5c8-7cd2c7398ad1"), ResponseIdentifier = Guid.Parse("7b0b625b-7994-446f-aeeb-d1831b558aa9"), ResponseDetail = "" },
                new GetExposure() { Id=Guid.Parse("cbcd00ad-9890-43c2-b4aa-74d3c866f2d0"), Id_GeneralInformation=Guid.Parse("63eaf912-e2fd-41da-ae58-7cd977061220"), QuestionIdentifier = Guid.Parse("cdafb83b-db6a-4a02-a78c-e1d37fe32465"), ResponseIdentifier = Guid.Parse("512d2651-18e7-45d6-84bb-62488d34c447"), ResponseDetail = "" },
                }
        };

        public static GetExposureQuestionAnswerQuery GetExposureQuestionAnswerQuery = new GetExposureQuestionAnswerQuery();

        public static List<GetExposureQuestionsAnswerResponse> GetExposureQuestionsAnswerResponse => new List<GetExposureQuestionsAnswerResponse>()
        {
          new GetExposureQuestionsAnswerResponse() {
                IdQuestion=Guid.Parse("ea67d16c-ba6e-41cb-aebd-3c141b82dc9b"),
                DecriptionQuestion="¿Ejerce algún grado de poder público?",
                Detalle=false,
                 Answers= new List<GetExposureAnswer>
                 {
                      new GetExposureAnswer{ IdAnswer=Guid.Parse("f2730e82-e7ff-4e47-8b6b-8fd398642d37"), DecriptionAnswer="NO" },
                      new GetExposureAnswer{ IdAnswer=Guid.Parse("816a9147-b439-4e16-9977-f7eeb3c44886"), DecriptionAnswer="SI" }
                 }
          },
          new GetExposureQuestionsAnswerResponse() {
                IdQuestion=Guid.Parse("8693ac3d-dc03-4963-9caf-4ec30854b27d"),
                DecriptionQuestion="¿Tiene vínculo con alguna persona públicamente expuesta?",
                Detalle=false,
                 Answers= new List<GetExposureAnswer>
                 {
                      new GetExposureAnswer{ IdAnswer=Guid.Parse("9f644040-5e85-4e37-8661-57c049f9a9c0"), DecriptionAnswer="NO" },
                      new GetExposureAnswer{ IdAnswer=Guid.Parse("a2ad0450-da31-42a4-ab60-92f078437292"), DecriptionAnswer="SI" }
                 }
          },
        };
    }
}