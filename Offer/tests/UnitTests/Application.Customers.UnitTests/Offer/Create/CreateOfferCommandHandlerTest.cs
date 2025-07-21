///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using MediatR;
using yourInvoice.Offer.Application.Offer.Create;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Offer.Create
{
    public class CreateOfferCommandHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IPayerRepository> _mockPayerRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMediator> _mockIMediator;
        private readonly Mock<ISystem> _mockISystem;
        private readonly CreateOfferCommandHandler _handler;

        public CreateOfferCommandHandlerTest()
        {
            _mockRepository = new Mock<IOfferRepository>();
            _mockPayerRepository = new Mock<IPayerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockIMediator = new Mock<IMediator>();
            _mockISystem = new Mock<ISystem>();
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new CreateOfferCommandHandler(_mockRepository.Object, _mockUnitOfWork.Object, _mockPayerRepository.Object, _mockIMediator.Object, _mockISystem.Object);
        }

        [Fact]
        public async Task HandleCreateOffer_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            yourInvoice.Offer.Domain.Payers.Payer payer = new(new Guid(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
                , It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            CreateOfferCommand command = new(new Guid());
            _mockPayerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(payer);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleCreateOffer_WhenPayerNoExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            CreateOfferCommand command = new(new Guid());
            _mockPayerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()));

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.PayerNotExist), result.FirstError.Description);
        }
    }
}