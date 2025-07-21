///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ErrorOr;
using FluentAssertions;
using Moq;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Application.Accounts.CreateAccount;
using yourInvoice.Link.Application.Accounts.Reject;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.UnitTest.Account
{
    public class RejectAccountCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IUnitOfWorkLink> unitOfWork;
        private readonly Mock<ICatalogBusiness> _catalogBusiness;
        private readonly RejectAccountCommandHandler _handler;

        public RejectAccountCommandHandlerTest()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            unitOfWork = new Mock<IUnitOfWorkLink>();
            _catalogBusiness = new Mock<ICatalogBusiness>();

            _handler = new RejectAccountCommandHandler(_mockAccountRepository.Object, unitOfWork.Object, _catalogBusiness.Object);
        }

        [Fact]
        public async Task HandleRejectAccount_AccountRejected_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CatalogItemInfo EmailPort = new CatalogItemInfo() { Name = "EMAIL Port", Descripton = "587" };
            CatalogItemInfo EMAILServer = new CatalogItemInfo() { Name = "EMAIL Server", Descripton = "smtp.gmail.com" };
            CatalogItemInfo EMAILUser = new CatalogItemInfo() { Name = "EMAIL User", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILPasProjrd = new CatalogItemInfo() { Name = "EMAIL PasProjrd", Descripton = "ghkohgjmxfepfnbb" };
            CatalogItemInfo EMAILFrom = new CatalogItemInfo() { Name = "EMAIL From", Descripton = "yourInvoicenotification@gmail.com" };
            CatalogItemInfo EMAILSender = new CatalogItemInfo() { Name = "EMAIL Sender", Descripton = "yourInvoicenotification@gmail.com" };

            RejectAccountCommand command = new(Guid.NewGuid());

            CreateAccountCommand acco = new(Guid.NewGuid(), "1", "2", "r", "n", "sc", "ln", "sln", Guid.NewGuid(), "1", "test@test.com", "1", Guid.NewGuid(), "1", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            Domain.Accounts.Account account = new(Guid.NewGuid(), acco.personTypeId, acco.nit, acco.digitVerify, acco.socialReason, acco.name, acco.secondName,
           acco.lastName, acco.secondLastName, acco.documentTypeId, acco.documentNumber, acco.email, acco.mobileNumber, acco.mobileCountryId, acco.phoneNumber,
           acco.phoneCountryId, acco.contactById, "", CatalogCode_StatusPreRegister.Pending, ExtensionFormat.DateTimeCO(), 0, ExtensionFormat.DateTimeCO());

            _catalogBusiness.Setup(x => x.ListByCatalogAsync("EMAILyourInvoice")).ReturnsAsync(new List<CatalogItemInfo>() { EmailPort, EMAILFrom, EMAILPasProjrd, EMAILSender, EMAILServer, EMAILUser });
            _catalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo() { Descripton = "100" });

            _mockAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Domain.Accounts.Queries.AccountResponse() { Email = "test@test.com" });
            _mockAccountRepository.Setup(x => x.UpdateAsync(It.IsAny<Domain.Accounts.Account>())).ReturnsAsync(true);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleRejectAccount_AccountNoExist_Error()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.

            RejectAccountCommand command = new(Guid.NewGuid());

            _mockAccountRepository.Setup(x => x.GetByIdAsync(Guid.NewGuid()));

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            Assert.Equal(GetErrorDescription(MessageCodes.AccountNotExist), result.FirstError.Description);
        }
    }
}