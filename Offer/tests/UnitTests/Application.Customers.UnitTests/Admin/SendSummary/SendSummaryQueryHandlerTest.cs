///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.Admin.ListPending;
using MediatR;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Admin.EmailToSeller;
using yourInvoice.Offer.Application.Admin.SendSummary;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Notifications;

namespace Application.Customer.UnitTest.Admin.SendSummary
{
    public class SendSummaryQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private readonly Mock<IDocumentRepository> _mockIDocumentRepository;
        private readonly Mock<IStorage> _mockIStorage;
        private readonly Mock<IMediator> _mockIMediator;

        private SendSummaryQueryHandler _handler;

        public SendSummaryQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _mockIDocumentRepository = new Mock<IDocumentRepository>();
            _mockIStorage = new Mock<IStorage>();
            _mockIMediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task HandSendSummary_WhenNotIs_empty()
        {
            _mockIDocumentRepository.Setup(s => s.GetDocumentsByOfferNumberAndTypeAsync(It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(SendSummaryData.GetDocuments);
            _mockIInvoiceDispersionRepository.Setup(s => s.ListPendingBuysAsync(It.IsAny<SearchInfo>())).ReturnsAsync(ListPendingData.GetListPendingResponse);
            _mockIStorage.Setup(s => s.DownloadAsync(It.IsAny<string>())).ReturnsAsync(SendSummaryData.GetStream);
            _mockIMediator.Setup(m => m.Send(It.IsAny<EmailToSellerAdminPurchasedCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Notification()).Verifiable("Notificacion enviada");
            _handler = new SendSummaryQueryHandler(_mockIInvoiceDispersionRepository.Object, _mockIDocumentRepository.Object, _mockIStorage.Object, _mockIMediator.Object);
            SendSummaryQuery query = new SendSummaryQuery(offerId: 370, new List<string>());
            var result = await _handler.Handle(query, default);
            Assert.True(result.Value);
        }
    }
}