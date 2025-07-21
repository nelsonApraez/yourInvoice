
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetBank;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;


using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetBankQueryHandlerTest
    {
        private readonly Mock<IBankInformationRepository> _mockBankInformationRepository;
        private readonly Mock<ISystem> _mockSystem;
        private GetBankQueryHandler _handler;

        public GetBankQueryHandlerTest()
        {
            _mockBankInformationRepository = new Mock<IBankInformationRepository>();
            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task Handler_Get_Bank_Sucess()
        {
            _mockBankInformationRepository.Setup(s => s.ExistsBankAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockBankInformationRepository.Setup(s => s.GetbankInformationAsync(It.IsAny<Guid>())).ReturnsAsync(Bank_FinancialData.GetBankResponse);

            _handler = new GetBankQueryHandler(_mockBankInformationRepository.Object, _mockSystem.Object);

            var command = Bank_FinancialData.GetBankQueryValid;
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Handler_Get_Bank_Query_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUserNotValid);
            _handler = new GetBankQueryHandler(_mockBankInformationRepository.Object, _mockSystem.Object);

            var command = Bank_FinancialData.GetBankQuery;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeFalse();
            Assert.Null(result.Value);
        }

    }
}