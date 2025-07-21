///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Offer.Invoice.Nullify;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.EventNotifications;
using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;

namespace Application.Customer.UnitTest.Offer.Invoice
{
    public class NullifyInvoceCommandHandlerTest
    {
        private readonly Mock<IInvoiceRepository> _mockIInvoiceRepository;
        private readonly Mock<IOfferRepository> _mockIOfferRepository;
        private readonly Mock<IDocumentRepository> _mockIDocumentRepository;
        private readonly Mock<IEventNotificationsRepository> _mockIEventNotificationsRepository;
        private readonly Mock<IInvoiceEventRepository> _mockIInvoiceEventRepository;
        private readonly Mock<IMoneyTransferRepository> _mockIMoneyTransferRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private NullifyInvoceCommandHandler _handler;

        public NullifyInvoceCommandHandlerTest()
        {
            mockStorage = new Mock<IStorage>();
            _mockIMoneyTransferRepository = new Mock<IMoneyTransferRepository>();
            _mockIInvoiceRepository = new Mock<IInvoiceRepository>();
            _mockIOfferRepository = new Mock<IOfferRepository>();
            _mockIDocumentRepository = new Mock<IDocumentRepository>();
            _mockIEventNotificationsRepository = new Mock<IEventNotificationsRepository>();
            _mockIInvoiceEventRepository = new Mock<IInvoiceEventRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new NullifyInvoceCommandHandler(_mockIOfferRepository.Object, _mockIInvoiceRepository.Object, _mockIDocumentRepository.Object,
                _mockIEventNotificationsRepository.Object, _mockIInvoiceEventRepository.Object, _mockUnitOfWork.Object, _mockIMoneyTransferRepository.Object, mockStorage.Object);
        }

        [Fact]
        public async Task HandleNullifyInvoce_WhenIdParameters_ShouldTrue()
        {
            NullifyInvoceCommand command = new NullifyInvoceCommand(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"));
            _mockIMoneyTransferRepository.Setup(x => x.GetAllByOfferId(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"))).ReturnsAsync(new List<MoneyTransfer>());
            _mockIDocumentRepository.Setup(y => y.GetDocumentsByOfferAndRelatedAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new List<Document>());
            _mockIDocumentRepository.Setup(y => y.GetAllDocumentsByOfferAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>());
            _mockIInvoiceRepository.Setup(c => c.FindByOfferId(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"))).ReturnsAsync(new List<yourInvoice.Offer.Domain.Invoices.Invoice>());
            _mockIOfferRepository.Setup(s => s.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await _handler.Handle(command, default);
            result.IsError.Should().BeFalse();
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleNullifyInvoce_WhenIdParameters_ShouldNull()
        {
            NullifyInvoceCommand command = null;
            var result = await _handler.Handle(command, default);
            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task HandleNullifyInvoce_WhenDelete_Information()
        {
            NullifyInvoceCommand command = new NullifyInvoceCommand(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"));
            _mockIMoneyTransferRepository.Setup(x => x.GetAllByOfferId(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"))).ReturnsAsync(new List<MoneyTransfer>());
            _mockIDocumentRepository.Setup(y => y.GetDocumentsByOfferAndRelatedAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new List<Document>());
            _mockIInvoiceRepository.Setup(c => c.FindByOfferId(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"))).ReturnsAsync(new List<yourInvoice.Offer.Domain.Invoices.Invoice>());
            _mockIDocumentRepository.Setup(y => y.GetAllDocumentsByOfferAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>());
            _mockIOfferRepository.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _mockIInvoiceRepository.Setup(s => s.DeleteAsync(It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);
            _mockIEventNotificationsRepository.Setup(s => s.NullyfyAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _mockIInvoiceEventRepository.Setup(s => s.NullyfyAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _mockIOfferRepository.Setup(s => s.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockUnitOfWork.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HandleNullifyInvoce_WhenNotDelete_Information()
        {
            NullifyInvoceCommand command = new NullifyInvoceCommand(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"));
            _mockIMoneyTransferRepository.Setup(x => x.GetAllByOfferId(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"))).ReturnsAsync(new List<MoneyTransfer>());
            _mockIDocumentRepository.Setup(y => y.GetDocumentsByOfferAndRelatedAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new List<Document>());
            _mockIInvoiceRepository.Setup(c => c.FindByOfferId(new Guid("C0DBBA66-E101-4CF2-A7FB-9B600522B840"))).ReturnsAsync(new List<yourInvoice.Offer.Domain.Invoices.Invoice>());
            _mockIDocumentRepository.Setup(y => y.GetAllDocumentsByOfferAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Document>());
            _mockIOfferRepository.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _mockIInvoiceRepository.Setup(s => s.DeleteAsync(It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);
            _mockIEventNotificationsRepository.Setup(s => s.NullyfyAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _mockIInvoiceEventRepository.Setup(s => s.NullyfyAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(-1);
            var result = await _handler.Handle(command, default);
            Assert.False(result.Value);
        }
    }
}