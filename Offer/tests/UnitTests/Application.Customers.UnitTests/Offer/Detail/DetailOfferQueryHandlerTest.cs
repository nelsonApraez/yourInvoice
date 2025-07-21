///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using Microsoft.AspNetCore.Http;
using yourInvoice.Offer.Application.Offer.Detail;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Offers;

namespace Application.Customer.UnitTest.Offer.Detail
{
    public class DetailOfferQueryHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockOfferRepository;
        private readonly Mock<ISystem> _mockISystem;
        private readonly Mock<IHttpContextAccessor> _mockIHttpContextAccessor;
        private DetailOfferQueryHandler _handler;

        public DetailOfferQueryHandlerTest()
        {
            byte[] dummy;
            _mockOfferRepository = new Mock<IOfferRepository>();
            _mockISystem = new Mock<ISystem>();
            _mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockSession = new Mock<ISession>();
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            mockSession.Setup(x => x.TryGetValue(It.IsAny<string>(), out dummy)).Returns(true);
            _mockIHttpContextAccessor.Setup(a => a.HttpContext.Session).Returns(mockSession.Object);
            _handler = new DetailOfferQueryHandler(_mockOfferRepository.Object, _mockISystem.Object, _mockIHttpContextAccessor.Object);
        }

        [Fact]
        public async Task HandleDetailOffer_When_IsEmpty()
        {
            DetailOfferQuery command = new DetailOfferQuery(Guid.Empty);
            var result = await _handler.Handle(command, default);
            Assert.True(result.FirstError.Type == ErrorType.Unexpected);
        }

        [Fact]
        public async Task HandleAll_When_Detail_GetInformations()
        {
            _mockOfferRepository.Setup(s => s.DetailAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(DatailOfferData.GetDetailOfferResponse);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new DetailOfferQueryHandler(_mockOfferRepository.Object, _mockISystem.Object, _mockIHttpContextAccessor.Object);
            DetailOfferQuery command = new DetailOfferQuery(Guid.NewGuid());
            var result = await _handler.Handle(command, default);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleAll_When_Detail_GetNull()
        {
            _mockOfferRepository.Setup(s => s.DetailAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(DatailOfferData.GetDetailOfferResponseNull);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new DetailOfferQueryHandler(_mockOfferRepository.Object, _mockISystem.Object, _mockIHttpContextAccessor.Object);
            DetailOfferQuery command = new DetailOfferQuery(Guid.NewGuid());
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value.PayerNit is null);
        }
    }
}