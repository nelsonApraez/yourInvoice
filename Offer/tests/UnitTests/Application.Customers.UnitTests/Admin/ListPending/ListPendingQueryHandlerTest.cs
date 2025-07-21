///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Admin.ListPending;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace Application.Customer.UnitTest.Admin.ListPending
{
    public class ListPendingQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private readonly Mock<ICatalogBusiness> _mockICatalogBusiness;
        private ListPendingQueryHandler _handler;

        public ListPendingQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _mockICatalogBusiness = new Mock<ICatalogBusiness>();
        }

        [Fact]
        public async Task HandlerListPending_WhenNotIs_empty()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.ListPendingBuysAsync(It.IsAny<SearchInfo>())).ReturnsAsync(ListPendingData.GetListPendingResponse);
            _mockICatalogBusiness.Setup(s => s.ListByCatalogAsync(It.IsAny<string>())).ReturnsAsync(ListPendingData.GetCatalogItemInfo);
            _handler = new ListPendingQueryHandler(_mockIInvoiceDispersionRepository.Object, _mockICatalogBusiness.Object);
            ListPendingQuery query = new ListPendingQuery(ListPendingData.GetSearchInfo);
            var result = await _handler.Handle(query, default);
            Assert.True(result.Value.Count > 0);
        }
    }
}