///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Offer.ListAll;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Offers;

namespace Application.Customer.UnitTest.Offer.ListAll
{
    public class ListAllOfferQueryHandlerTest
    {
        private readonly Mock<IOfferRepository> _mockOfferRepository;
        private readonly Mock<ISystem> _mockISystem;
        private ListAllOfferQueryHandler _handler;

        public ListAllOfferQueryHandlerTest()
        {
            _mockOfferRepository = new Mock<IOfferRepository>();
            _mockISystem = new Mock<ISystem>();
            _handler = new ListAllOfferQueryHandler(_mockOfferRepository.Object, _mockISystem.Object);
        }

        [Fact]
        public async Task HandleAll_When_ListAllByUser_GetInformations()
        {
            _mockOfferRepository.Setup(s => s.ListAllByUserAsync(It.IsAny<SearchInfo>(), It.IsAny<Guid>())).ReturnsAsync(ListAllOfferData.GetListAllOfferResponse);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ListAllOfferQueryHandler(_mockOfferRepository.Object, _mockISystem.Object);
            ListAllOfferQuery command = new ListAllOfferQuery(ListAllOfferData.GetSearchInfo);
            var result = await _handler.Handle(command, default);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleAll_When_ListAllByUser_GetNull()
        {
            _mockOfferRepository.Setup(s => s.ListAllByUserAsync(It.IsAny<SearchInfo>(), It.IsAny<Guid>())).ReturnsAsync(ListAllOfferData.GetListAllOfferResponseNull);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ListAllOfferQueryHandler(_mockOfferRepository.Object, _mockISystem.Object);
            ListAllOfferQuery command = new ListAllOfferQuery(ListAllOfferData.GetSearchInfo);
            var result = await _handler.Handle(command, default);
            Assert.True(result.Value.Data is null);
        }
    }
}