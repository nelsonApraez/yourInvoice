///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.LinkingProcess.UpdateWorking;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdateWorkingCommandHandlerTest
    {
        private readonly Mock<IWorkingInformationRepository> _mockWorkingInformationRepository;

        private UpdateWorkingCommandHandler _handler;

        public UpdateWorkingCommandHandlerTest()
        {
            _mockWorkingInformationRepository = new Mock<IWorkingInformationRepository>();
        }

        [Fact]
        public async Task HandleUpdateExposurre_Sucess()
        {
            _mockWorkingInformationRepository.Setup(s => s.UpdateWorkingAsync(It.IsAny<WorkingInformation>())).ReturnsAsync(true);

            _handler = new UpdateWorkingCommandHandler(_mockWorkingInformationRepository.Object);

            var command = WorkingData.UpdateWorkingCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }
    }
}