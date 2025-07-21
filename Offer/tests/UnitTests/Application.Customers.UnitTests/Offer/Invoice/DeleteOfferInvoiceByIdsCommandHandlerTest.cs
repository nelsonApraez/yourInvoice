///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Offer.Invoice.DeleteByIds;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;

namespace Application.Customer.UnitTest.Offer.Invoice
{
    public class DeleteOfferInvoiceByIdsCommandHandlerTest
    {
        private readonly Mock<IInvoiceRepository> _mockIInvoiceRepository;
        private readonly Mock<IDocumentRepository> _mockIDocumentRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IInvoiceEventRepository> _mockInvoiceEventRepository;
        private readonly DeleteOfferInvoiceByIdsCommandHandler _handler;
        private readonly Mock<IOfferRepository> _mockIOfferRepository;

        public DeleteOfferInvoiceByIdsCommandHandlerTest()
        {
            mockStorage = new Mock<IStorage>();
            _mockIInvoiceRepository = new Mock<IInvoiceRepository>();
            _mockIDocumentRepository = new Mock<IDocumentRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockInvoiceEventRepository = new Mock<IInvoiceEventRepository>();
            _mockIOfferRepository = new Mock<IOfferRepository>();
            _handler = new DeleteOfferInvoiceByIdsCommandHandler(_mockIInvoiceRepository.Object, _mockIDocumentRepository.Object, _mockUnitOfWork.Object, mockStorage.Object, _mockInvoiceEventRepository.Object, _mockIOfferRepository.Object);
        }

        [Fact]
        public async Task HandleDeleteInvoice_WhenListIdsParameters_ShouldTrue()
        {
            DeleteOfferInvoiceByIdsCommand command = new DeleteOfferInvoiceByIdsCommand(new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() });

            _mockIOfferRepository.Setup(x => x.OfferIsInProgressByInvoiceIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockIInvoiceRepository.Setup(y => y.GetById(It.IsAny<Guid>())).ReturnsAsync(new List<yourInvoice.Offer.Domain.Invoices.Invoice>()
            { new yourInvoice.Offer.Domain.Invoices.Invoice(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"), new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"),
            null,null,null,It.IsAny<Guid>(),new DateTime(),new DateTime(),1,1,It.IsAny<Guid>(),1,null,null,1)});
            _mockIDocumentRepository.Setup(y => y.GetDocumentsByOfferAndRelatedAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new List<Document>());

            _mockInvoiceEventRepository.Setup(y => y.DeleteAsync(It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);
            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }
    }
}