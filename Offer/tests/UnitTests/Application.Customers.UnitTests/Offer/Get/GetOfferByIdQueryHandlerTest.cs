///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Offer.GetById;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Offer.Get
{
    public class GetOfferByIdQueryHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly GetOfferByIdQueryHandler _handler;

        public GetOfferByIdQueryHandlerTest()
        {
            _mockRepository = new Mock<IOfferRepository>();

            _handler = new GetOfferByIdQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleGetOfferById_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetOfferByIdQuery command = new(new Guid());
            _mockRepository.Setup(x => x.GetByIdWithNamesAsync(It.IsAny<Guid>())).ReturnsAsync(new
                yourInvoice.Offer.Domain.Offers.Queries.GetOfferResponse("123", "payername", "sellername", "estado", Guid.Empty, 1, 1, 1));

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleGetOfferById_WhenOfferNoExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetOfferByIdQuery command = new(new Guid());
            _mockRepository.Setup(x => x.GetByIdWithNamesAsync(It.IsAny<Guid>()));

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            Assert.Equal(GetErrorDescription(MessageCodes.OfferNotExist), result.FirstError.Description);
        }
    }
}