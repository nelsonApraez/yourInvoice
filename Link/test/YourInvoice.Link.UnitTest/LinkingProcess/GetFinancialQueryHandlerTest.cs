
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetFinancial;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;


using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetFinancialQueryHandlerTest
    {
        private readonly Mock<IFinancialInformationRepository> _mockFinancialInformationRepository;
        private readonly Mock<ISystem> _mockSystem;
        private GetFinancialQueryHandler _handler;

        public GetFinancialQueryHandlerTest()
        {
            _mockFinancialInformationRepository = new Mock<IFinancialInformationRepository>();
            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task Handler_Get_Financial_Sucess()
        {
            _mockFinancialInformationRepository.Setup(s => s.ExistsFinancialAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockFinancialInformationRepository.Setup(s => s.GetFinancialInformationAsync(It.IsAny<Guid>())).ReturnsAsync(Bank_FinancialData.GetFinancialResponse);

            _handler = new GetFinancialQueryHandler(_mockFinancialInformationRepository.Object, _mockSystem.Object);

            var command = Bank_FinancialData.GetFinancialQueryValid;
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Handler_Get_Financial_Query_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUserNotValid);
            _handler = new GetFinancialQueryHandler(_mockFinancialInformationRepository.Object, _mockSystem.Object);

            var command = Bank_FinancialData.GetFinancialQuery;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeFalse();
            Assert.Null(result.Value);
        }


    }
}