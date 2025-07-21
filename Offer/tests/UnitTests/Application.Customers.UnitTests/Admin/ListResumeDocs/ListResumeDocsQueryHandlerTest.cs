///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.Admin.ListResumeDocs;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Admin.ListResumeDocs
{
    public class ListResumeDocsQueryHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly ListResumeDocsQueryHandler _handler;

        public ListResumeDocsQueryHandlerTest()
        {
            _mockRepository = new Mock<IOfferRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();

            _handler = new ListResumeDocsQueryHandler(_mockDocumentRepository.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task HandleListResumeDocsQuery_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            ListResumeDocsQuery command = new(It.IsAny<int>());
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(It.IsAny<int>())).ReturnsAsync(new
                yourInvoice.Offer.Domain.Offer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress));
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>() {
                new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.DocumentsUploadByUserOnResume,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.CommercialOffer,true,"url")});

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleListResumeDocsQuery_WhenOfferNoExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            ListResumeDocsQuery command = new(It.IsAny<int>());

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.OfferNotExist), result.FirstError.Description);
        }
    }
}