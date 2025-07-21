///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetExposureQuestionResponse;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetExposureQuestionAnswerQueryHandlerTest
    {
        private readonly Mock<IExposureInformationRepository> _mockExposureInformationRepository;
        private readonly Mock<ISystem> _mockSystem;
        private GetExposureQuestionAnswerQueryHandler _handler;

        public GetExposureQuestionAnswerQueryHandlerTest()
        {
            _mockExposureInformationRepository = new Mock<IExposureInformationRepository>();
            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task Handler_GetExposureQuestionAnswerQuery_Sucess()
        {
            _mockExposureInformationRepository.Setup(s => s.ExistsExposureAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockExposureInformationRepository.Setup(s => s.GetExposureQuestionAnswerAsync(It.IsAny<string>(), It.IsAny<List<Guid>>())).ReturnsAsync(ExposureData.GetExposureQuestionsAnswerResponse);

            _handler = new GetExposureQuestionAnswerQueryHandler(_mockExposureInformationRepository.Object);

            var command = ExposureData.GetExposureQuestionAnswerQuery;
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }
    }
}