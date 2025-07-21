///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Admin.ListPurchased;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace Application.Customer.UnitTest.Admin.ListPurchased
{
    public class ListPurchasedQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private ListPurchasedQueryHandler _handler;

        public ListPurchasedQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
        }

        [Fact]
        public async Task HandlerListPurchased_WhenNotIs_empty()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.ListPurchasedAsync(It.IsAny<SearchInfo>())).ReturnsAsync(ListPurchasedData.GetListPurchasedResponse);
            _handler = new ListPurchasedQueryHandler(_mockIInvoiceDispersionRepository.Object);
            ListPurchasedQuery query = new ListPurchasedQuery(ListPurchasedData.GetSearchInfo);
            var result = await _handler.Handle(query, default);
            Assert.True(result.Value.Count > 0);
        }
    }
}