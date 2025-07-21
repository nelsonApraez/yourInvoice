///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Admin.ListTransactions;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace Application.Customer.UnitTest.Admin.ListTransaction
{
    public class ListTransactionsQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private ListTransactionsQueryHandler _handler;

        public ListTransactionsQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _handler = new ListTransactionsQueryHandler(_mockIInvoiceDispersionRepository.Object);
        }

        [Fact]
        public async Task HeaderTransaction_WhenTransactionIdParameters_IsEmpty()
        {
            ListTransactionsQuery command = new ListTransactionsQuery(0);
            var result = await _handler.Handle(command, default);
            Assert.True(result.FirstError.Type == ErrorType.Unexpected);
        }

        [Fact]
        public async Task HeaderTransaction_WhenTransactionId_GetInformations()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.ListTransactionsAsync(It.IsAny<int>())).ReturnsAsync(new List<yourInvoice.Offer.Domain.Admin.Queries.ListTransactionsResponse>());
            _handler = new ListTransactionsQueryHandler(_mockIInvoiceDispersionRepository.Object);
            ListTransactionsQuery command = new ListTransactionsQuery(It.IsAny<int>());
            var result = await _handler.Handle(command, default);
            Assert.NotNull(result.Value);
        }
    }
}