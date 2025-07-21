///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Application.LinkingProcess.UpdateLegalSAGRILAFT;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdateSagrilaftCommandHandlerTest
    {
        private readonly Mock<ILegalSAGRILAFTRepository> _mockSAGRILAFTInformationRepository;

        private UpdateSagrilaftCommandHandler _handler;

        public UpdateSagrilaftCommandHandlerTest()
        {
            _mockSAGRILAFTInformationRepository = new Mock<ILegalSAGRILAFTRepository>();
        }

        [Fact]
        public async Task HandleUpdateSagrilaft_Sucess()
        {
            _mockSAGRILAFTInformationRepository.Setup(s => s.UpdateSagrilaftAsync(It.IsAny<LegalSAGRILAFT>())).ReturnsAsync(true);

            _handler = new UpdateSagrilaftCommandHandler(_mockSAGRILAFTInformationRepository.Object);

            var command = SagrilaftData.GetUpdateSagrilaftCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }
    }
}