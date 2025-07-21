///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Admin.HeaderOffer;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Admin.HeaderOffer
{
    public class HeaderOfferQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private HeaderOfferQueryHandler _handler;

        public HeaderOfferQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _handler = new HeaderOfferQueryHandler(_mockIInvoiceDispersionRepository.Object);
        }

        [Fact]
        public async Task HeaderOfferQuery_WhenOfferNoExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            HeaderOfferQuery command = new(It.IsAny<int>());
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.OfferNotExist), result.FirstError.Description);
        }

        [Fact]
        public async Task HeaderOffer_WhenOfferId_GetInformations()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.GetHeaderOfferAsync(It.IsAny<int>())).ReturnsAsync(new yourInvoice.Offer.Domain.Admin.Queries.HeaderTransactionResponse());
            _handler = new HeaderOfferQueryHandler(_mockIInvoiceDispersionRepository.Object);
            HeaderOfferQuery command = new HeaderOfferQuery(It.IsAny<int>());
            var result = await _handler.Handle(command, default);
            Assert.NotNull(result.Value);
        }
    }
}