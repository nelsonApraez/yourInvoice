///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ErrorOr;
using FluentAssertions;
using MediatR;
using Moq;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Link.Application.Accounts.CreateAccount;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;
using yourInvoice.Link.Domain.AccountRoles;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Offer.Domain.Notifications;
using yourInvoice.Offer.Domain.Users;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.UnitTest.Account
{
    public class CreateAccountCommandHandlerTest
    {
        private readonly Mock<IAccountRoleRepository> accountRoleRepository;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IUnitOfWorkLink> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICatalogBusiness> _catalogBusiness;
        private readonly Mock<IMediator> _mockIMediator;
        private readonly CreateAccountCommandHandler _handler;

        public CreateAccountCommandHandlerTest()
        {
            accountRoleRepository = new Mock<IAccountRoleRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWorkLink>();
            _userRepository = new Mock<IUserRepository>();
            _catalogBusiness = new Mock<ICatalogBusiness>();
            _mockIMediator = new Mock<IMediator>();

            _handler = new CreateAccountCommandHandler(_mockAccountRepository.Object, _mockUnitOfWork.Object, accountRoleRepository.Object, _userRepository.Object, _catalogBusiness.Object,
                  _mockIMediator.Object);
        }

        [Fact]
        public async Task HandleCreateAccount_AccountRegistered_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CreateAccountCommand command = CreateData.CreateAccountRequest;
            _mockAccountRepository.Setup(x => x.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockAccountRepository.Setup(x => x.Add(CreateData.AccountCreated)).Returns(CreateData.AccountCreated);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

            _catalogBusiness.Setup(x => x.ListByCatalogAsync("EMAILyourInvoice")).ReturnsAsync(new List<CatalogItemInfo>() { CreateData.EmailPort, CreateData.EmailFrom, CreateData.EmailPasProjrd, CreateData.EmailSender, CreateData.EmailServer, CreateData.EmailUser });
            _catalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });
            _catalogBusiness.Setup(x => x.GetByIdAsync(CatalogCode_DatayourInvoice.EmailAdmin)).ReturnsAsync(new CatalogItemInfo() { Descripton = "test@test.com" });
            _userRepository.Setup(x => x.GetEmailRoleAsync(CatalogCode_UserRole.Administrator)).ReturnsAsync("test@test.com");
            _mockIMediator.Setup(m => m.Send(It.IsAny<ChangeLinkStatusCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleCreateAccount_AccountExist_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CreateAccountCommand command = CreateData.CreateAccountRequest;
            _mockAccountRepository.Setup(x => x.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.AccountExist), result.FirstError.Description);
        }

        [Fact]
        public async Task HandleCreateAccount_ParameterEmpty_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CreateAccountCommand command = CreateData.CreateAccountRequestNameEmpty;

            _mockAccountRepository.Setup(x => x.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.ParameterEmpty, "Nombre"), result.FirstError.Description);
        }
    }
}