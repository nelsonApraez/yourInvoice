///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using yourInvoice.Offer.Application.Buyer.Resume;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Common.Business.CatalogModule;

namespace Application.Customer.UnitTest.Buyer.Resume
{
    public class ResumeQueryHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockRepository;
        private readonly Mock<IDocumentRepository> _mockDocumentRepository;
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private readonly Mock<ISystem> _mockISystem;
        private ResumeQueryHandler _handler;

        public ResumeQueryHandlerTest()
        {
            _mockRepository = new Mock<IOfferRepository>();
            _mockDocumentRepository = new Mock<IDocumentRepository>();
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _mockISystem = new Mock<ISystem>();
        }

        [Fact]
        public async Task HandlerResumeOffer_WhenNumberOffer_GetInformations()
        {
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(It.IsAny<int>())).ReturnsAsync(new
            yourInvoice.Offer.Domain.Offer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress));
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>() {
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.CommercialOffer,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.Endorsement,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.EndorsementNotification,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.MoneyTransferInstruction,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<Guid>(),"nombre", CatalogCode_DocumentType.CommercialOfferBuyer,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<Guid>(),"nombre", CatalogCode_DocumentType.PurchaseCertificate,true,"url")});
            _mockIInvoiceDispersionRepository.Setup(s => s.GetResumeAsync(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(ResumeData.GetResumeResponse);

            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ResumeQueryHandler(_mockIInvoiceDispersionRepository.Object, _mockDocumentRepository.Object, _mockRepository.Object, _mockISystem.Object);
            ResumeQuery command = new ResumeQuery(ResumeData.NumberOffer);
            var result = await _handler.Handle(command, default);
            Assert.False(string.IsNullOrEmpty(result.Value.NamePayer));
        }

        [Fact]
        public async Task HandlerResumeOffer_WhenNumberOffer_NotGetInformations()
        {
            _mockRepository.Setup(x => x.GetByConsecutiveAsync(It.IsAny<int>())).ReturnsAsync(new
             yourInvoice.Offer.Domain.Offer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), DateTime.UtcNow, DateTime.UtcNow, "", CatalogCode_OfferStatus.InProgress));
            _mockDocumentRepository.Setup(x => x.GetAllDocumentsByOfferAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>() {
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.CommercialOffer,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.Endorsement,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.EndorsementNotification,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), null,"nombre", CatalogCode_DocumentType.MoneyTransferInstruction,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<Guid>(),"nombre", CatalogCode_DocumentType.CommercialOfferBuyer,true,"url"),
            new Document(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<Guid>(),"nombre", CatalogCode_DocumentType.PurchaseCertificate,true,"url")});
            _mockIInvoiceDispersionRepository.Setup(s => s.GetResumeAsync(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(ResumeData.GetResumeResponseEmpty);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ResumeQueryHandler(_mockIInvoiceDispersionRepository.Object, _mockDocumentRepository.Object, _mockRepository.Object, _mockISystem.Object);
            ResumeQuery command = new ResumeQuery(ResumeData.NumberOffer);
            var result = await _handler.Handle(command, default);
            Assert.True(string.IsNullOrEmpty(result.Value.NamePayer));
        }
    }
}