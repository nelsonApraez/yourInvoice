///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Offer.GetBase64Document;
using yourInvoice.Offer.Domain.Documents;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Offer.GetBase64Document
{
    public class GetBase64DocumentQueryHandlerTest
    {
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IStorage> _mockStorage;
        private readonly Mock<ICatalogBusiness> _mockCatalogBusiness;

        private readonly GetBase64DocumentQueryHandler _handler;

        public GetBase64DocumentQueryHandlerTest()
        {
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockStorage = new Mock<IStorage>();
            _mockCatalogBusiness = new Mock<ICatalogBusiness>();

            _handler = new GetBase64DocumentQueryHandler(_mockDocumentRepository.Object, _mockStorage.Object, _mockCatalogBusiness.Object);
        }

        private static byte[] ObtenerDatosImagen()
        {
            // Ejemplo: Una imagen PNG simple con un solo píxel blanco (1x1 píxel)
            byte[] datosImagen = new byte[]
            {
            137, 80, 78, 71, 13, 10, 26, 10, // Encabezado PNG
            0, 0, 0, 13, // Tamaño de la sección IHDR
            73, 72, 68, 82, // "IHDR" (Identificador de la sección)
            0, 0, 0, 1, // Ancho (1 píxel)
            0, 0, 0, 1, // Altura (1 píxel)
            8, // Profundidad de bits (8 bits)
            6, // Tipo de color (RGBA)
            0, // Método de compresión (ninguno)
            0, // Método de filtrado (ninguno)
            0, // Método de interlazado (ninguno)
            255, 255, 255, 255 // Píxel blanco (RGBA)
            };

            return datosImagen;
        }

        [Fact]
        public async Task HandleGetBase64Document_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetBase64DocumentQuery command = new(new Guid());
            _mockDocumentRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>() { new Document(It.IsAny<Guid>(),
                It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.CommercialOffer,true,"url")  });
            _mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new yourInvoice.Common.Entities.CatalogItemInfo() { Descripton = "des" });
            _mockStorage.Setup(x => x.DownloadAsync(It.IsAny<string>())).ReturnsAsync(new MemoryStream(ObtenerDatosImagen()));

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleGetBase64DocumentQuery_WhenDocumentNoExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            GetBase64DocumentQuery command = new(new Guid());
            _mockDocumentRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>());
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.DocumentNotExist), result.FirstError.Description);
        }
    }
}