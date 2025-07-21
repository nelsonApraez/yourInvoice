///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.LinkingProcess.CreateLegalRepresentativeTaxAuditor;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.UnitTest.LinkingProcess
{
    public class CreateLegalRepresentativeTaxAuditorCommandHandlerTest
    {
        private readonly Mock<ILegalRepresentativeTaxAuditorRepository> _mockLegalRepresentativeTaxAuditorRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWorkLink;
        private CreateLegalRepresentativeTaxAuditorCommandHandler _handler;

        public CreateLegalRepresentativeTaxAuditorCommandHandlerTest()
        {
            _mockLegalRepresentativeTaxAuditorRepository = new Mock<ILegalRepresentativeTaxAuditorRepository>();
            _mockUnitOfWorkLink = new Mock<IUnitOfWorkLink>();
            _mockUnitOfWorkLink.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }

        [Fact]
        public async Task Handle_Create_LegalRepresentativeTaxAuditor_Sucess()
        {
            _mockLegalRepresentativeTaxAuditorRepository.Setup(s => s.ExistseLegalRepresentativeTaxAuditorRepositoryAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            _mockLegalRepresentativeTaxAuditorRepository.Setup(s => s.CreateLegalRepresentativeTaxAuditorRepositoryAsync(It.IsAny<LegalRepresentativeTaxAuditor>())).ReturnsAsync(true);
            _handler = new CreateLegalRepresentativeTaxAuditorCommandHandler(_mockLegalRepresentativeTaxAuditorRepository.Object, _mockUnitOfWorkLink.Object);

            var command = LegalRepresentativeTaxAuditorData.CreateLegalRepresentativeTaxAuditorCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handle_Update_LegalRepresentativeTaxAuditor_Sucess()
        {
            _mockLegalRepresentativeTaxAuditorRepository.Setup(s => s.ExistseLegalRepresentativeTaxAuditorRepositoryAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalRepresentativeTaxAuditorRepository.Setup(s => s.CreateLegalRepresentativeTaxAuditorRepositoryAsync(It.IsAny<LegalRepresentativeTaxAuditor>())).ReturnsAsync(true);
            _handler = new CreateLegalRepresentativeTaxAuditorCommandHandler(_mockLegalRepresentativeTaxAuditorRepository.Object, _mockUnitOfWorkLink.Object);

            var command = LegalRepresentativeTaxAuditorData.CreateLegalRepresentativeTaxAuditorCommand;
            var result = await _handler.Handle(command, default);

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Handle_validate_user_LegalRepresentativeTaxAuditor()
        {
            _mockLegalRepresentativeTaxAuditorRepository.Setup(s => s.ExistseLegalRepresentativeTaxAuditorRepositoryAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockLegalRepresentativeTaxAuditorRepository.Setup(s => s.CreateLegalRepresentativeTaxAuditorRepositoryAsync(It.IsAny<LegalRepresentativeTaxAuditor>())).ReturnsAsync(true);
            _handler = new CreateLegalRepresentativeTaxAuditorCommandHandler(_mockLegalRepresentativeTaxAuditorRepository.Object, _mockUnitOfWorkLink.Object);
            var command = LegalRepresentativeTaxAuditorData.CreateLegalRepresentativeTaxAuditorCommandEmpy;
            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeTrue();
            Assert.False(result.Value);
        }
    }
}