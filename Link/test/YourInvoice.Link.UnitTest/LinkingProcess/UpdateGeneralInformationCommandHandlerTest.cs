using Moq;
using yourInvoice.Link.Application.LinkingProcess.UpdateGeneralInformation;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdateGeneralInformationCommandHandlerTest
    {
        private readonly Mock<IGeneralInformationRepository> _mockGeneralInformationRepository;

        private UpdateGeneralInformationCommandHandler _handler;

        public UpdateGeneralInformationCommandHandlerTest()
        {
            _mockGeneralInformationRepository = new Mock<IGeneralInformationRepository>();
        }

        [Fact]
        public async Task UpdateGeneralInformation_Success()
        {
            _mockGeneralInformationRepository.Setup(s => s.UpdateGeneralInformationAsync(It.IsAny<GeneralInformationResponse>())).ReturnsAsync(true);

            _handler = new UpdateGeneralInformationCommandHandler(_mockGeneralInformationRepository.Object);

            var command = GeneralInformation.UpdateGeneralInformationCommandCommandValid;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task UpdateGeneralInformation_NotSuccess()
        {
            _mockGeneralInformationRepository.Setup(s => s.UpdateGeneralInformationAsync(It.IsAny<GeneralInformationResponse>())).ReturnsAsync(true);

            _handler = new UpdateGeneralInformationCommandHandler(_mockGeneralInformationRepository.Object);

            var command = GeneralInformation.UpdateGeneralInformationCommandCommandNoValid;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }




    }
}
