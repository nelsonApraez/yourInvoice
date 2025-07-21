///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalGeneralInformation;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateLegalGeneralInformationCommandHandlerTest
    {
        private readonly Mock<ILegalGeneralInformationRepository> _mockLegalGeneralInformationRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWorkLink;
        private CreateLegalGeneralInformationCommandHandler _handler;

        public CreateLegalGeneralInformationCommandHandlerTest()
        {
            _mockLegalGeneralInformationRepository = new Mock<ILegalGeneralInformationRepository>();
            _mockUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
            _mockUnitOfWorkLink.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }

        [Fact]
        public async Task Handle_Create_LegalGeneralInformation_Sucess()
        {
            _mockLegalGeneralInformationRepository.Setup(s => s.ExistseLegalGeneralInformationAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockLegalGeneralInformationRepository.Setup(s => s.ExistsAccountLegalAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalGeneralInformationRepository.Setup(s => s.CreateLegalGeneralInformationAsync(It.IsAny<LegalGeneralInformation>())).ReturnsAsync(true);
            _handler = new CreateLegalGeneralInformationCommandHandler(_mockLegalGeneralInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = LegalGeneralInformationData.CreateLegalGeneralInformationCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handle_Update_LegalGeneralInformation_Sucess()
        {
            _mockLegalGeneralInformationRepository.Setup(s => s.ExistseLegalGeneralInformationAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalGeneralInformationRepository.Setup(s => s.ExistsAccountLegalAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalGeneralInformationRepository.Setup(s => s.UpdateLegalGeneralInformationAsync(It.IsAny<LegalGeneralInformation>())).ReturnsAsync(true);
            _handler = new CreateLegalGeneralInformationCommandHandler(_mockLegalGeneralInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = LegalGeneralInformationData.CreateLegalGeneralInformationCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handle_validate_user_LegalGeneralInformation()
        {
            _mockLegalGeneralInformationRepository.Setup(s => s.ExistseLegalGeneralInformationAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalGeneralInformationRepository.Setup(s => s.ExistsAccountLegalAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalGeneralInformationRepository.Setup(s => s.UpdateLegalGeneralInformationAsync(It.IsAny<LegalGeneralInformation>())).ReturnsAsync(true);
            _handler = new CreateLegalGeneralInformationCommandHandler(_mockLegalGeneralInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = LegalGeneralInformationData.CreateLegalGeneralInformationCommandEmpy;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }

        [Fact]
        public async Task Handle_Not_Exists_Account_LegalGeneralInformation()
        {
            _mockLegalGeneralInformationRepository.Setup(s => s.ExistseLegalGeneralInformationAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalGeneralInformationRepository.Setup(s => s.ExistsAccountLegalAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);
            _mockLegalGeneralInformationRepository.Setup(s => s.UpdateLegalGeneralInformationAsync(It.IsAny<LegalGeneralInformation>())).ReturnsAsync(true);
            _handler = new CreateLegalGeneralInformationCommandHandler(_mockLegalGeneralInformationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = LegalGeneralInformationData.CreateLegalGeneralInformationCommand;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }
    }
}