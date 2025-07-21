
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Offer.Domain;
using yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFT;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class GetSagrilaftQueryHandlerTest
    {
        private readonly Mock<ILegalSAGRILAFTRepository> _mockSAGRILAFTInformationRepository;
        private readonly Mock<ISystem> _mockSystem;
        private GetSagrilaftQueryHandler _handler;

        public GetSagrilaftQueryHandlerTest()
        {
            _mockSAGRILAFTInformationRepository = new Mock<ILegalSAGRILAFTRepository>();
            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(SagrilaftData.GetUser);
        }

        [Fact]
        public async Task HandleGetExposurreQuery_Sucess()
        {
            _mockSAGRILAFTInformationRepository.Setup(s => s.ExistsSagrilaftAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockSAGRILAFTInformationRepository.Setup(s => s.GetSagrilaftAsync(It.IsAny<Guid>())).ReturnsAsync(SagrilaftData.GetSagrilaftResponse);

            _handler = new GetSagrilaftQueryHandler(_mockSAGRILAFTInformationRepository.Object, _mockSystem.Object);

            var command = SagrilaftData.GetGetSagrilaftQueryValid;
            var result = await _handler.Handle(command, default);

            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleGetSagrilaftQuery_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(SagrilaftData.GetUserNotValid);
            _handler = new GetSagrilaftQueryHandler(_mockSAGRILAFTInformationRepository.Object, _mockSystem.Object);

            var command = SagrilaftData.GetGetSagrilaftQuery;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.Null(result.Value);
        }
    }
}