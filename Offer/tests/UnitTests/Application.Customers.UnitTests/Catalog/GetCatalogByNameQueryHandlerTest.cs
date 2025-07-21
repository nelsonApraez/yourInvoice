///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Catalog.GetByName;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Catalog
{
    public class GetCatalogByNameQueryHandlerTest
    {
        private readonly Mock<ICatalogBusiness> _catalogBusiness;
        private readonly GetCatalogByNameQueryHandler _handler;

        public GetCatalogByNameQueryHandlerTest()
        {
            _catalogBusiness = new Mock<ICatalogBusiness>();

            _handler = new GetCatalogByNameQueryHandler(_catalogBusiness.Object);
        }

        [Fact]
        public async Task HandleGetOfferById_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetCatalogByNameQuery command = new("123");
            _catalogBusiness.Setup(x => x.ListByCatalogAsync(It.IsAny<string>())).ReturnsAsync(new List<CatalogItemInfo>() { new CatalogItemInfo() });

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
            GetCatalogByNameQuery command = new("123");
            _catalogBusiness.Setup(x => x.ListByCatalogAsync(It.IsAny<string>()));

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.NotFound);
            Assert.Equal(GetErrorDescription(MessageCodes.CatalogNotExist), result.FirstError.Description);
        }
    }
}