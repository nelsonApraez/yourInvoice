///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using MediatR;
using Moq;
using yourInvoice.Link.Application.Accounts.Approve;
using yourInvoice.Link.Application.Accounts.Approve.EmailApprove;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Offer.Domain.Notifications;

namespace yourInvoice.Link.UnitTest.Account
{
    public class ApproveAccountCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _mockIAccountRepository;
        private readonly Mock<IGeneralInformationRepository> _mockIGeneralInformationRepository;
        private readonly Mock<ILegalGeneralInformationRepository> _mockILegalGeneralInformationRepository;
        private readonly Mock<IMediator> _mockIMediator;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWork;
        private ApproveAccountCommandHandler _handler;

        public ApproveAccountCommandHandlerTest()
        {
            _mockIAccountRepository = new Mock<IAccountRepository>();
            _mockIGeneralInformationRepository = new Mock<IGeneralInformationRepository>();
            _mockIMediator = new Mock<IMediator>();
            _mockILegalGeneralInformationRepository = new Mock<ILegalGeneralInformationRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWorkLink>();

            _handler = new ApproveAccountCommandHandler(_mockIAccountRepository.Object, _mockIGeneralInformationRepository.Object, _mockIMediator.Object, _mockILegalGeneralInformationRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task HandleApprovedAccount_Sucess()
        {
            //Arrange
            ApproveAccountCommand command = new(Guid.NewGuid());
            _mockIAccountRepository.Setup(s => s.GetAccountIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApproveData.GetAccountResponse);
            _mockIGeneralInformationRepository.Setup(s => s.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(ApproveData.GetGeneralInformationResponse);
            
            _mockIAccountRepository.Setup(s => s.UpdateStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>())).ReturnsAsync(true);
            _mockIGeneralInformationRepository.Setup(s => s.UpdateStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            _mockIMediator.Setup(m => m.Send(It.IsAny<EmailApproveCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");
            
            //Act
            var result = await _handler.Handle(command, default);
            //Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleApprovedAccount_NoSuccess_AccountEmpty()
        {
            //Arrange
            ApproveAccountCommand command = new(Guid.NewGuid());
            _mockIAccountRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApproveData.GetAccountResponseEmpty);

            //Act
            var result = await _handler.Handle(command, default);
            //Assert
            Assert.False(result.Value);
        }

        [Fact]
        public async Task HandleApprovedAccount_NoSuccess_GeneralInfoEmpty()
        {
            //Arrange
            ApproveAccountCommand command = new(Guid.NewGuid());
            _mockIAccountRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApproveData.GetAccountResponse);
            _mockIGeneralInformationRepository.Setup(s => s.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(ApproveData.GetGeneralInformationResponseEmpty);

            _handler = new ApproveAccountCommandHandler(_mockIAccountRepository.Object, _mockIGeneralInformationRepository.Object, _mockIMediator.Object, _mockILegalGeneralInformationRepository.Object, _mockUnitOfWork.Object);

            //Act
            var result = await _handler.Handle(command, default);
            //Assert
            Assert.False(result.Value);
        }
    }
}