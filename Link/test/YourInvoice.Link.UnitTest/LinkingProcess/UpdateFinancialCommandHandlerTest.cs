
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.LinkingProcess.UpdateFinancial;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdateFinancialCommandHandlerTest
    {
        private readonly Mock<IFinancialInformationRepository> _mockFinancialInformationRepository;

        private UpdateFinancialCommandHandler _handler;

        public UpdateFinancialCommandHandlerTest()
        {
            _mockFinancialInformationRepository = new Mock<IFinancialInformationRepository>();
        }

        [Fact]
        public async Task HandleUpdateExposurre_Sucess()
        {
            _mockFinancialInformationRepository.Setup(s => s.UpdateFinancialAsync(It.IsAny<FinancialInformation>())).ReturnsAsync(true);

            _handler = new UpdateFinancialCommandHandler(_mockFinancialInformationRepository.Object);

            var command = Bank_FinancialData.UpdateFinancialCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }
    }
}