///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.Admin.ListDocs;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Admin.ListDocs
{
    public class ListAdminDocsQueryHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IInvoiceDispersionRepository> _mockInvoiceDispersionRepository;
        private readonly ListAdminDocsQueryHandler _handler;

        public ListAdminDocsQueryHandlerTest()
        {
            _mockRepository = new Mock<IOfferRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();

            _handler = new ListAdminDocsQueryHandler(_mockDocumentRepository.Object, _mockRepository.Object, _mockInvoiceDispersionRepository.Object);
        }

        [Fact]
        public async Task HandleListAdminDocsQuery_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            ListAdminDocsQuery command = new(It.IsAny<int>());

            _mockInvoiceDispersionRepository.Setup(x => x.GetByTransactionNumberAsync(It.IsAny<int>())).ReturnsAsync(new InvoiceDispersion(
            Guid.NewGuid(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), new DateTime(), new DateTime(), 1, "1", "1", 1, 1, 10, 10, new char(), true, Guid.NewGuid(), new DateTime(), 0, new DateTime(), true,
            new DateTime(), new DateTime(), 0, new DateTime(), Guid.NewGuid()));
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(It.IsAny<int>())).ReturnsAsync(new
                yourInvoice.Offer.Domain.Offer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress));
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>() {
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.CommercialOffer,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.Endorsement,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.EndorsementNotification,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.MoneyTransferInstruction,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<Guid>(),"nombre", CatalogCode_DocumentType.CommercialOfferBuyer,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<Guid>(),"nombre", CatalogCode_DocumentType.PurchaseCertificate,true,"url")});

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleListAdminDocsQuery_WhenOfferNoExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            ListAdminDocsQuery command = new(It.IsAny<int>());
            _mockInvoiceDispersionRepository.Setup(x => x.GetByTransactionNumberAsync(It.IsAny<int>())).ReturnsAsync(new InvoiceDispersion(
                    Guid.NewGuid(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), new DateTime(), new DateTime(), 1, "1", "1", 1, 1, 10, 10, new char(), true, Guid.NewGuid(), new DateTime(), 0, new DateTime(), true
                    , new DateTime(), new DateTime(), 0, new DateTime(), Guid.NewGuid()));
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