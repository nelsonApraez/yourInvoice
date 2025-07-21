///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Buyer.ResumeInvoice;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace Application.Customer.UnitTest.Buyer.ResumeInvoice
{
    public class ResumeInvoiceQueryHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;
        private readonly Mock<ISystem> _mockISystem;
        private ResumeInvoiceQueryHandler _handler;

        public ResumeInvoiceQueryHandlerTest()
        {
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _mockISystem = new Mock<ISystem>();
        }

        [Fact]
        public async Task HandlerResumeInvoice_WhenNumberOfferParameters_IsEmpty()
        {
            ResumeInvoiceQuery command = new ResumeInvoiceQuery(ResumeInvoiceData.GeSearchInfo, 0);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ResumeInvoiceQueryHandler(_mockIInvoiceDispersionRepository.Object, _mockISystem.Object);
            var result = await _handler.Handle(command, default);
            Assert.True(result.FirstError.Type == ErrorType.Validation);
        }

        [Fact]
        public async Task HandlerResumeInvoice_WhenNumberOffer_GetInformations()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.ResumeInvoiceAsync(It.IsAny<SearchInfo>(), It.IsAny<int>(), It.IsAny<Guid>())).ReturnsAsync(ResumeInvoiceData.GetResumeInvoiceResponse);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ResumeInvoiceQueryHandler(_mockIInvoiceDispersionRepository.Object, _mockISystem.Object);
            ResumeInvoiceQuery command = new ResumeInvoiceQuery(ResumeInvoiceData.GeSearchInfo, ResumeInvoiceData.NumberOffer);
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value.Count > 0);
        }

        [Fact]
        public async Task HandlerResumeInvoice_WhenNumberOffer_NotGetInformations()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.ResumeInvoiceAsync(It.IsAny<SearchInfo>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(ResumeInvoiceData.GetResumeInvoiceResponseEmpty);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ResumeInvoiceQueryHandler(_mockIInvoiceDispersionRepository.Object, _mockISystem.Object);
            ResumeInvoiceQuery command = new ResumeInvoiceQuery(ResumeInvoiceData.GeSearchInfo, ResumeInvoiceData.NumberOffer);
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value.Count == 0);
        }
    }
}