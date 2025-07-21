///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.CreateExposure;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateExposureCommandHandlerTest
    {
        private readonly Mock<IExposureInformationRepository> _mockExposureInformationRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWorkLink;
        private readonly Mock<ISystem> _mockSystem;
        private CreateExposureCommandHandler _handler;

        public CreateExposureCommandHandlerTest()
        {
            _mockExposureInformationRepository = new Mock<IExposureInformationRepository>();

            _mockUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
            _mockUnitOfWorkLink.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task HandleCreateExposurre_Sucess()
        {
            _mockExposureInformationRepository.Setup(s => s.ExistsExposureAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockExposureInformationRepository.Setup(s => s.CreateExposureAsync(It.IsAny<IEnumerable<ExposureInformation>>())).ReturnsAsync(true);

            _handler = new CreateExposureCommandHandler(_mockExposureInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = ExposureData.GetCreateExposureCommandValid;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleCreateExposurre_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUserNotValid);
            _handler = new CreateExposureCommandHandler(_mockExposureInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = ExposureData.GetCreateExposureCommand;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }

        [Fact]
        public async Task HandleCreateExposurre_exists_data_validation()
        {
            _mockExposureInformationRepository.Setup(s => s.ExistsExposureAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _handler = new CreateExposureCommandHandler(_mockExposureInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = ExposureData.GetCreateExposureCommand;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }
    }
}