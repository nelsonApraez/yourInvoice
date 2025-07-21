
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.CreateFinancial;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateFinancialCommandHandlerTest
    {
        private readonly Mock<IFinancialInformationRepository> _mockFinancialInformationRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWorkLink;
        private readonly Mock<ISystem> _mockSystem;
        private CreateFinancialCommandHandler _handler;

        public CreateFinancialCommandHandlerTest()
        {
            _mockFinancialInformationRepository = new Mock<IFinancialInformationRepository>();

            _mockUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
            _mockUnitOfWorkLink.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task Handler_CreateFinancial_Sucess()
        {
            _mockFinancialInformationRepository.Setup(s => s.ExistsFinancialAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockFinancialInformationRepository.Setup(s => s.CreateFinancialAsync(It.IsAny<FinancialInformation>())).ReturnsAsync(true);

            _handler = new CreateFinancialCommandHandler(_mockFinancialInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = Bank_FinancialData.CreateFinancialCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handler_CreateFinancial_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUserNotValid);
            _handler = new CreateFinancialCommandHandler(_mockFinancialInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = Bank_FinancialData.CreateFinancialCommandNoValid;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }

        [Fact]
        public async Task Handler_CreateFinancial_Exists_Data_Financial_validation()
        {
            _mockFinancialInformationRepository.Setup(s => s.ExistsFinancialAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _handler = new CreateFinancialCommandHandler(_mockFinancialInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = Bank_FinancialData.CreateFinancialCommand;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }
    }
}