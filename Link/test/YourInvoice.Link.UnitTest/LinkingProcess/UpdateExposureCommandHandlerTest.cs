///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.LinkingProcess.UpdateExposure;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdateExposureCommandHandlerTest
    {
        private readonly Mock<IExposureInformationRepository> _mockExposureInformationRepository;

        private UpdateExposureCommandHandler _handler;

        public UpdateExposureCommandHandlerTest()
        {
            _mockExposureInformationRepository = new Mock<IExposureInformationRepository>();
        }

        [Fact]
        public async Task HandleUpdateExposurre_Sucess()
        {
            _mockExposureInformationRepository.Setup(s => s.UpdateExposureAsync(It.IsAny<ExposureInformation>())).ReturnsAsync(true);

            _handler = new UpdateExposureCommandHandler(_mockExposureInformationRepository.Object);

            var command = ExposureData.GetUpdateExposureCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }
    }
}