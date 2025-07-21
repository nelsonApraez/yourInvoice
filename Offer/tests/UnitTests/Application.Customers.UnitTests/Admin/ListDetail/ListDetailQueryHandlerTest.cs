///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Admin.ListDetail;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace Application.Customer.UnitTest.Admin.ListDetail
{
    public class ListDetailQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private ListDetailQueryHandler _handler;

        public ListDetailQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
        }

        [Fact]
        public async Task HandlerListPending_WhenNotIs_empty()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.ListDetailAsync(It.IsAny<int>(), It.IsAny<SearchInfo>())).ReturnsAsync(ListDetailData.GetListDetailResponse);
            _handler = new ListDetailQueryHandler(_mockIInvoiceDispersionRepository.Object);
            ListDetailQuery query = new ListDetailQuery(offerId: 370, ListDetailData.GetSearchInfo);
            var result = await _handler.Handle(query, default);
            Assert.True(result.Value.Count > 0);
        }
    }
}