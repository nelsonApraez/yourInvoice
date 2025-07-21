///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetExposure;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetExposureQueryHandlerTest
    {
        private readonly Mock<IExposureInformationRepository> _mockExposureInformationRepository;
        private readonly Mock<ISystem> _mockSystem;
        private GetExposureQueryHandler _handler;

        public GetExposureQueryHandlerTest()
        {
            _mockExposureInformationRepository = new Mock<IExposureInformationRepository>();
            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task HandleGetExposurreQuery_Sucess()
        {
            _mockExposureInformationRepository.Setup(s => s.ExistsExposureAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockExposureInformationRepository.Setup(s => s.GetExposureAsync(It.IsAny<Guid>())).ReturnsAsync(ExposureData.GetExposureResponse);

            _handler = new GetExposureQueryHandler(_mockExposureInformationRepository.Object, _mockSystem.Object);

            var command = ExposureData.GetGetExposureQueryValid;
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleGetExposureQuery_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUserNotValid);
            _handler = new GetExposureQueryHandler(_mockExposureInformationRepository.Object, _mockSystem.Object);

            var command = ExposureData.GetGetExposureQuery;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.Null(result.Value);
        }
    }
}