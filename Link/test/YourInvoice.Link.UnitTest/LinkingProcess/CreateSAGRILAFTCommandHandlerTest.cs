
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Offer.Domain;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalSAGRILAFT;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateSAGRILAFTCommandHandlerTest
    {
        private readonly Mock<ILegalSAGRILAFTRepository> _mockSAGRILAFTInformationRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWorkLink;
        private readonly Mock<ISystem> _mockSystem;
        private CreateSagrilaftCommandHandler _handler;

        public CreateSAGRILAFTCommandHandlerTest()
        {
            _mockSAGRILAFTInformationRepository = new Mock<ILegalSAGRILAFTRepository>();

            _mockUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
            _mockUnitOfWorkLink.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mockSystem = new Mock<ISystem>();
            _mockSystem.Setup(x => x.User).Returns(SagrilaftData.GetUser);
        }

        [Fact]
        public async Task HandleCreateExposurre_Sucess()
        {
            _mockSAGRILAFTInformationRepository.Setup(s => s.ExistsSagrilaftAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockSAGRILAFTInformationRepository.Setup(s => s.CreateSagrilaftAsync(It.IsAny<IEnumerable<LegalSAGRILAFT>>())).ReturnsAsync(true);

            _handler = new CreateSagrilaftCommandHandler(_mockSAGRILAFTInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = SagrilaftData.GetCreateSagrilaftCommandValid;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleCreateExposurre_Not_Current_User_validation()
        {
            _mockSystem.Setup(x => x.User).Returns(SagrilaftData.GetUserNotValid);
            _handler = new CreateSagrilaftCommandHandler(_mockSAGRILAFTInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = SagrilaftData.GetCreateSagrilaftCommand;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }

        [Fact]
        public async Task HandleCreateExposurre_exists_data_validation()
        {
            _mockSAGRILAFTInformationRepository.Setup(s => s.ExistsSagrilaftAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            _handler = new CreateSagrilaftCommandHandler(_mockSAGRILAFTInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = SagrilaftData.GetCreateSagrilaftCommand;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }
    }
}