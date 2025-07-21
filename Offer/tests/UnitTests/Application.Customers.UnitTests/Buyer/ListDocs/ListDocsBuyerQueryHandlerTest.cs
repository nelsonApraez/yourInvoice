///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.Buyer.ListDocs;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Buyer.ListDocs
{
    public class ListDocsBuyerQueryHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<ISystem> _mockISystem;
        private ListDocsBuyerQueryHandler _handler;

        public ListDocsBuyerQueryHandlerTest()
        {
            _mockRepository = new Mock<IOfferRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockISystem = new Mock<ISystem>();
        }

        [Fact]
        public async Task HandleListDocsBuyerQuery_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            ListDocsBuyerQuery command = new(It.IsAny<int>());
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(It.IsAny<int>())).ReturnsAsync(new
                yourInvoice.Offer.Domain.Offer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress));
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>() {
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), UserData.GetUser.Id,"nombre", CatalogCode_DocumentType.CommercialOffer,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), UserData.GetUser.Id,"nombre", CatalogCode_DocumentType.Endorsement,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), UserData.GetUser.Id,"nombre", CatalogCode_DocumentType.EndorsementNotification,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), UserData.GetUser.Id,"nombre", CatalogCode_DocumentType.MoneyTransferInstruction,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), UserData.GetUser.Id,"nombre", CatalogCode_DocumentType.CommercialOfferBuyer,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), UserData.GetUser.Id,"nombre", CatalogCode_DocumentType.PurchaseCertificate,true,"url")});
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);

            _handler = new ListDocsBuyerQueryHandler(_mockDocumentRepository.Object, _mockRepository.Object, _mockISystem.Object);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleListDocsBuyerQuery_WhenOfferNoExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            ListDocsBuyerQuery command = new(It.IsAny<int>());

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ListDocsBuyerQueryHandler(_mockDocumentRepository.Object, _mockRepository.Object, _mockISystem.Object);
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