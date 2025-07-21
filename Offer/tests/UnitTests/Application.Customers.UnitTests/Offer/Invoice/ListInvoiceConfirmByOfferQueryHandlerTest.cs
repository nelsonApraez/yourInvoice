///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Offer.Invoice.ListConfirm;
using yourInvoice.Offer.Domain.Invoices;

namespace Application.Customer.UnitTest.Offer.Invoice
{
    public class ListInvoiceConfirmByOfferQueryHandlerTest
    {
        private readonly Mock<IInvoiceRepository> _mockRepository;
        private readonly ListInvoiceConfirmByOfferQueryHandler _handler;

        public ListInvoiceConfirmByOfferQueryHandlerTest()
        {
            _mockRepository = new Mock<IInvoiceRepository>();

            _handler = new ListInvoiceConfirmByOfferQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleListInvoiceConfirmByOffer_WhenReturnsData()
        {
            SearchInfo searchInfo = new() { ColumnOrder = "InvoiceNumber", OrderType = "asc", PageSize = 3, StartIndex = 0 };

            ListInvoiceConfirmByOfferQuery command = new(Guid.NewGuid(), searchInfo);
            _mockRepository.Setup(x => x.ListConfirmAsync(It.IsAny<Guid>(), searchInfo)).ReturnsAsync(InvoiceData.GetInvoiceListConfirmDataResponse);

            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleListInvoiceConfirmByOffer_WhenReturnsEmpty()
        {
            SearchInfo searchInfo = new() { ColumnOrder = "InvoiceNumber", OrderType = "asc", PageSize = 3, StartIndex = 0 };

            ListInvoiceConfirmByOfferQuery command = new(Guid.NewGuid(), searchInfo);
            _mockRepository.Setup(x => x.ListConfirmAsync(It.IsAny<Guid>(), searchInfo)).ReturnsAsync(InvoiceData.GetInvoiceListConfirmDataResponseEmpy);

            var result = await _handler.Handle(command, default);

            result.IsError.Should().BeFalse();
            Assert.True(result.Value?.Data is null);
        }

        [Fact]
        public async Task HandleListInvoiceConfirmByOffer_WhenReturnsNull()
        {
            SearchInfo searchInfo = new() { ColumnOrder = "InvoiceNumber", OrderType = "asc", PageSize = 3, StartIndex = 0 };

            ListInvoiceConfirmByOfferQuery command = new(Guid.NewGuid(), searchInfo);
            _mockRepository.Setup(x => x.ListConfirmAsync(It.IsAny<Guid>(), searchInfo)).ReturnsAsync(InvoiceData.GetInvoiceListConfirmDataResponseNull);

            var result = await _handler.Handle(command, default);

            Assert.True(result.Value is null);
        }
    }
}