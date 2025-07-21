
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Moq;
using yourInvoice.Link.Application.LinkingProcess.UpdateBank;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class UpdateBankCommandHandlerTest
    {
        private readonly Mock<IBankInformationRepository> _mockBankInformationRepository;

        private UpdateBankCommandHandler _handler;

        public UpdateBankCommandHandlerTest()
        {
            _mockBankInformationRepository = new Mock<IBankInformationRepository>();
        }

        [Fact]
        public async Task HandleUpdateExposurre_Sucess()
        {
            _mockBankInformationRepository.Setup(s => s.UpdateBankAsync(It.IsAny<BankInformation>())).ReturnsAsync(true);

            _handler = new UpdateBankCommandHandler(_mockBankInformationRepository.Object);

            var command = Bank_FinancialData.UpdateBankCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }
    }
}