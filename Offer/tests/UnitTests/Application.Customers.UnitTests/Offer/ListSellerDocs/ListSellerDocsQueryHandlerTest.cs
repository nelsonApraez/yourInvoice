///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using Microsoft.AspNetCore.Http;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.Offer.ListSellerDocs;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;

namespace Application.Customer.UnitTest.Offer.ListSellerDocs
{
    public class ListSellerDocsQueryHandlerTest
    {
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<ISystem> _mockISystem;
        private readonly Mock<IHttpContextAccessor> _mockIHttpContextAccessor;
        private ListSellerDocsQueryHandler _handler;

        public ListSellerDocsQueryHandlerTest()
        {
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockISystem = new Mock<ISystem>();
            _mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task HandleListSellerDocsQuery_Sucess()
        {
            var mockSession = new Mock<ISession>();
            byte[] dummy;
            mockSession.Setup(x => x.TryGetValue(It.IsAny<string>(), out dummy)).Returns(true);

            _mockIHttpContextAccessor
                 .Setup(a => a.HttpContext.Session)
                .Returns(mockSession.Object);

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ListSellerDocsQueryHandler(_mockDocumentRepository.Object, _mockISystem.Object, _mockIHttpContextAccessor.Object);

            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            ListSellerDocsQuery command = new(new Guid());
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

            Assert.NotNull(result.Value);
        }
    }
}