///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Admin.HeaderTransaction;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace Application.Customer.UnitTest.Admin.HeaderTransaction
{
    public class HeaderTransactionQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private HeaderTransactionQueryHandler _handler;

        public HeaderTransactionQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _handler = new HeaderTransactionQueryHandler(_mockIInvoiceDispersionRepository.Object);
        }

        [Fact]
        public async Task HeaderTransaction_WhenTransactionIdParameters_IsEmpty()
        {
            HeaderTransactionQuery command = new HeaderTransactionQuery(0);
            var result = await _handler.Handle(command, default);
            Assert.True(result.FirstError.Type == ErrorType.Unexpected);
        }

        [Fact]
        public async Task HeaderTransaction_WhenTransactionId_GetInformations()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.GetHeaderTransactionAsync(It.IsAny<int>())).ReturnsAsync(new yourInvoice.Offer.Domain.Admin.Queries.HeaderTransactionResponse());
            _handler = new HeaderTransactionQueryHandler(_mockIInvoiceDispersionRepository.Object);
            HeaderTransactionQuery command = new HeaderTransactionQuery(It.IsAny<int>());
            var result = await _handler.Handle(command, default);
            Assert.NotNull(result.Value);
        }
    }
}