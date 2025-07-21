
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.CreateBank;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateBankCommandHandlerTest
    {
        private readonly Mock<IBankInformationRepository> _mockBankInformationRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWorkLink;
        private readonly Mock<ISystem> _mockSystem;
        private CreateBankCommandHandler _handler;

        public CreateBankCommandHandlerTest()
        {
            _mockBankInformationRepository = new Mock<IBankInformationRepository>();

            _mockUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
            _mockUnitOfWorkLink.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task Handler_CreateBank_Sucess()
        {
            _mockBankInformationRepository.Setup(s => s.ExistsBankAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockBankInformationRepository.Setup(s => s.CreateBankAsync(It.IsAny<BankInformation>())).ReturnsAsync(true);

            _handler = new CreateBankCommandHandler(_mockBankInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = Bank_FinancialData.CreateBankCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handler_CreateBank_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUserNotValid);
            _handler = new CreateBankCommandHandler(_mockBankInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = Bank_FinancialData.CreateBankCommandNoValid;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }

        [Fact]
        public async Task Handler_CreateBank_Exists_Data_Bank_validation()
        {
            _mockBankInformationRepository.Setup(s => s.ExistsBankAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _handler = new CreateBankCommandHandler(_mockBankInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = Bank_FinancialData.CreateBankCommand;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }
    }
}