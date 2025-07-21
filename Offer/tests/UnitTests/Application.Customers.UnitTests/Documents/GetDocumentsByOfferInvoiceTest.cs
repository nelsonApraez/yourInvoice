///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Documents.GetDocumentsByOfferInvoice;
using yourInvoice.Offer.Domain.Documents;

namespace Application.Customer.UnitTest.Documents
{
    public class GetDocumentsByOfferInvoiceTest
    {
        private readonly Mock<IDocumentRepository> _mockIDocumentRepository;
        private readonly Mock<IStorage> _mockIStorage;
        private GetDocumentsByOfferInvoiceQueryHandler _handler;

        public GetDocumentsByOfferInvoiceTest()
        {
            _mockIDocumentRepository = new Mock<IDocumentRepository>();
            _mockIStorage = new Mock<IStorage>();
        }

        [Fact]
        public async Task HandleGetDocumentsByOfferInvoice_WhenParameter_IsEmpty()
        {
            _mockIDocumentRepository.Setup(s => s.GetDocumentsByOfferInvoiceAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(DocumentData.GetDocumentsEmpy);
            _mockIStorage.Setup(s => s.GenerateSecureDownloadUrlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(DocumentData.GetUrlToken);
            _handler = new GetDocumentsByOfferInvoiceQueryHandler(_mockIDocumentRepository.Object, _mockIStorage.Object);

            GetDocumentsByOfferInvoiceQuery query = new GetDocumentsByOfferInvoiceQuery(DocumentData.GetDocumentByOfferInvoiceRequestExtensionTypeFileEmpty);
            var result = await _handler.Handle(query, default);

            result.FirstError.Type.Should().Be(ErrorType.Validation);
        }

        [Fact]
        public async Task HandleGetDocumentsByOfferInvoice_WhenParameters_Not_IsEmpty()
        {
            _mockIDocumentRepository.Setup(s => s.GetDocumentsByOfferInvoiceAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(DocumentData.GetDocuments);
            _mockIStorage.Setup(s => s.GenerateSecureDownloadUrlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(DocumentData.GetUrlToken);
            _handler = new GetDocumentsByOfferInvoiceQueryHandler(_mockIDocumentRepository.Object, _mockIStorage.Object);

            GetDocumentsByOfferInvoiceQuery query = new GetDocumentsByOfferInvoiceQuery(DocumentData.GetDocumentByOfferInvoiceRequest);
            var result = await _handler.Handle(query, default);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleGetDocumentsByOfferInvoice_WhenReturnRepository_IsEmpty()
        {
            _mockIDocumentRepository.Setup(s => s.GetDocumentsByOfferInvoiceAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(DocumentData.GetDocumentsEmpy);
            _mockIStorage.Setup(s => s.GenerateSecureDownloadUrlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(DocumentData.GetUrlToken);
            _handler = new GetDocumentsByOfferInvoiceQueryHandler(_mockIDocumentRepository.Object, _mockIStorage.Object);

            GetDocumentsByOfferInvoiceQuery query = new GetDocumentsByOfferInvoiceQuery(DocumentData.GetDocumentByOfferInvoiceRequest);
            var result = await _handler.Handle(query, default);
            Assert.True(result.Value.Count == 0);
        }
    }
}