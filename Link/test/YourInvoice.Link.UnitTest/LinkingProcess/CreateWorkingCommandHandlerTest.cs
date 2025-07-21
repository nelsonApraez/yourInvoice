///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.CreateWorking;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateWorkingCommandHandlerTest
    {
        private readonly Mock<IWorkingInformationRepository> _mockWorkingInformationRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWorkLink;
        private readonly Mock<ISystem> _mockSystem;
        private CreateWorkingCommandHandler _handler;

        public CreateWorkingCommandHandlerTest()
        {
            _mockWorkingInformationRepository = new Mock<IWorkingInformationRepository>();

            _mockUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
            _mockUnitOfWorkLink.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task Handler_CreateWorking_Sucess()
        {
            _mockWorkingInformationRepository.Setup(s => s.ExistsWorkingAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockWorkingInformationRepository.Setup(s => s.CreateWorkingAsync(It.IsAny<WorkingInformation>())).ReturnsAsync(true);

            _handler = new CreateWorkingCommandHandler(_mockWorkingInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = WorkingData.CreateWorkingCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handler_CreateWorking_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUserNotValid);
            _handler = new CreateWorkingCommandHandler(_mockWorkingInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = WorkingData.CreateWorkingCommandNoValid;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }

        [Fact]
        public async Task Handler_CreateWorking_Exists_Data_Working_validation()
        {
            _mockWorkingInformationRepository.Setup(s => s.ExistsWorkingAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _handler = new CreateWorkingCommandHandler(_mockWorkingInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = WorkingData.CreateWorkingCommand;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }
    }
}