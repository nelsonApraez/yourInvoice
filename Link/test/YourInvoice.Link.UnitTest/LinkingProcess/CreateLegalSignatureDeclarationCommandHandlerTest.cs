///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalSignatureDeclaration;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateLegalSignatureDeclarationCommandHandlerTest
    {
        private readonly Mock<ILegalSignatureDeclarationRepository> _mockLegalSignatureDeclarationRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWorkLink;
        private CreateLegalSignatureDeclarationCommandHandler _handler;

        public CreateLegalSignatureDeclarationCommandHandlerTest()
        {
            _mockLegalSignatureDeclarationRepository = new Mock<ILegalSignatureDeclarationRepository>();
            _mockUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
            _mockUnitOfWorkLink.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }

        [Fact]
        public async Task Handle_Create_LegalSignatureDeclaration_Sucess()
        {
            _mockLegalSignatureDeclarationRepository.Setup(s => s.ExistsLegalSignatureDeclarationAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockLegalSignatureDeclarationRepository.Setup(s => s.CreateLegalSignatureDeclarationAsync(It.IsAny<LegalSignatureDeclaration>())).ReturnsAsync(true);
            _handler = new CreateLegalSignatureDeclarationCommandHandler(_mockLegalSignatureDeclarationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = LegalSignatureDeclarationData.CreateLegalSignatureDeclarationCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handle_Update_LegalSignatureDeclaration_Sucess()
        {
            _mockLegalSignatureDeclarationRepository.Setup(s => s.ExistsLegalSignatureDeclarationAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalSignatureDeclarationRepository.Setup(s => s.CreateLegalSignatureDeclarationAsync(It.IsAny<LegalSignatureDeclaration>())).ReturnsAsync(true);
            _handler = new CreateLegalSignatureDeclarationCommandHandler(_mockLegalSignatureDeclarationRepository.Object, _mockUnitOfWorkLink.Object);

            var command = LegalSignatureDeclarationData.CreateLegalSignatureDeclarationCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handle_validate_user_LegalSignatureDeclaration()
        {
            _mockLegalSignatureDeclarationRepository.Setup(s => s.ExistsLegalSignatureDeclarationAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalSignatureDeclarationRepository.Setup(s => s.CreateLegalSignatureDeclarationAsync(It.IsAny<LegalSignatureDeclaration>())).ReturnsAsync(true);
            _handler = new CreateLegalSignatureDeclarationCommandHandler(_mockLegalSignatureDeclarationRepository.Object, _mockUnitOfWorkLink.Object);
            var command = LegalSignatureDeclarationData.CreateLegalSignatureDeclarationCommandEmpy;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }
    }
}