///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.GetWorking;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetWorkingQueryHandlerTest
    {
        private readonly Mock<IWorkingInformationRepository> _mockWorkingInformationRepository;
        private readonly Mock<ISystem> _mockSystem;
        private GetWorkingQueryHandler _handler;

        public GetWorkingQueryHandlerTest()
        {
            _mockWorkingInformationRepository = new Mock<IWorkingInformationRepository>();
            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUser);
        }

        [Fact]
        public async Task Handler_Get_Working_Sucess()
        {
            _mockWorkingInformationRepository.Setup(s => s.ExistsWorkingAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockWorkingInformationRepository.Setup(s => s.GetWorkingAsync(It.IsAny<Guid>())).ReturnsAsync(WorkingData.GetWorkingResponse);

            _handler = new GetWorkingQueryHandler(_mockWorkingInformationRepository.Object, _mockSystem.Object);

            var command = WorkingData.GetWorkingQueryValid;
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Handler_Get_Working_Query_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(ExposureData.GetUserNotValid);
            _handler = new GetWorkingQueryHandler(_mockWorkingInformationRepository.Object, _mockSystem.Object);

            var command = WorkingData.GetWorkingQuery;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.Null(result.Value);
        }
    }
}