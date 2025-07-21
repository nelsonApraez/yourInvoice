///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ErrorOr;
using FluentAssertions;
using Moq;
using yourInvoice.Link.Application.Accounts.GetAccount;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Accounts.Queries;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.UnitTest.Account
{
    public class GetAccountQueryHandlerTest
    {
        private readonly Mock<IAccountRepository> _mockRepository;
        private readonly GetAccountQueryHandler _handler;

        public GetAccountQueryHandlerTest()
        {
            _mockRepository = new Mock<IAccountRepository>();

            _handler = new GetAccountQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleGetAccountQuery_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetAccountQuery command = new(new Guid());
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new AccountResponse());

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleGetOfferById_WhenAccountNoExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetAccountQuery command = new(new Guid());
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

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