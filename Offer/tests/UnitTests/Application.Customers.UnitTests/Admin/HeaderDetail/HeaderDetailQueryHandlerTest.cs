///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Admin.Header;
using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Admin.HeaderDetail
{
    public class HeaderDetailQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private HeaderDetailQueryHandler _handler;

        public HeaderDetailQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _handler = new HeaderDetailQueryHandler(_mockIInvoiceDispersionRepository.Object);
        }

        [Fact]
        public async Task HeaderOfferQuery_WhenOfferNoExist_ShouldReturnValidationError()
        {
            HeaderDetailQuery command = new(It.IsAny<int>());
            var result = await _handler.Handle(command, default);
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.OfferNotExist), result.FirstError.Description);
        }

        [Fact]
        public async Task HeaderOffer_WhenOfferId_GetInformations()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.GetHeaderDetailAsync(It.IsAny<int>())).ReturnsAsync(new HeaderDetailResponse() { NroOffer = 1, PayerName = "Nombre pagador", SellerName = "Nombre del vendedor" });
            _handler = new HeaderDetailQueryHandler(_mockIInvoiceDispersionRepository.Object);
            HeaderDetailQuery command = new HeaderDetailQuery(It.IsAny<int>());
            var result = await _handler.Handle(command, default);
            Assert.NotNull(result.Value);
        }
    }
}