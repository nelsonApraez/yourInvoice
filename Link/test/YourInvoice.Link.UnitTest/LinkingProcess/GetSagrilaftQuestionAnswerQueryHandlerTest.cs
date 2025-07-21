
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFTQuestionResponse;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetSagrilaftQuestionAnswerQueryHandlerTest
    {
        private readonly Mock<ILegalSAGRILAFTRepository> _mockSAGRILAFTInformationRepository;
        private readonly Mock<ISystem> _mockSystem;
        private GetSagrilaftQuestionAnswerQueryHandler _handler;

        public GetSagrilaftQuestionAnswerQueryHandlerTest()
        {
            _mockSAGRILAFTInformationRepository = new Mock<ILegalSAGRILAFTRepository>();
            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(SagrilaftData.GetUser);
        }

        [Fact]
        public async Task Handler_GetSagrilaftQuestionAnswerQuery_Sucess()
        {
            _mockSAGRILAFTInformationRepository.Setup(s => s.ExistsSagrilaftAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockSAGRILAFTInformationRepository.Setup(s => s.GetSagrilaftQuestionAnswerAsync(It.IsAny<string>(), It.IsAny<List<Guid>>())).ReturnsAsync(SagrilaftData.GetSagrilaftQuestionsAnswerResponse);

            _handler = new GetSagrilaftQuestionAnswerQueryHandler(_mockSAGRILAFTInformationRepository.Object);

            var command = SagrilaftData.GetSagrilaftQuestionAnswerQuery;
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }
    }
}