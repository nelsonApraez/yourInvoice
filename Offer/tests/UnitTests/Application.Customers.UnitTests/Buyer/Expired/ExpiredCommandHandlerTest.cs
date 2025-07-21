///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Application.Customer.UnitTest.User;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Offer.Application.Buyer.Expired;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Notifications;
using yourInvoice.Offer.Domain.Primitives;

namespace Application.Customer.UnitTest.Buyer.Expired
{
    public class ExpiredCommandHandlerTest
    {
        private readonly Mock<IInvoiceDispersionRepository> _mockIInvoiceDispersionRepository;

        private readonly Mock<INotificationRepository> _mockINotificationRepository;
        private readonly Mock<ICatalogBusiness> _mockICatalogBusiness;
        private readonly Mock<IUnitOfWork> _mockIUnitOfWork;
        private readonly Mock<ISystem> _mockISystem;
        private ExpiredCommandHandler _handler;

        public ExpiredCommandHandlerTest()
        {
            _mockICatalogBusiness = new Mock<ICatalogBusiness>();
            _mockIUnitOfWork = new Mock<IUnitOfWork>();
            _mockIInvoiceDispersionRepository = new Mock<IInvoiceDispersionRepository>();
            _mockINotificationRepository = new Mock<INotificationRepository>();
            _mockISystem = new Mock<ISystem>();
        }

        [Fact]
        public async Task HandExpired_WhenIs_empty()
        {
            _mockIInvoiceDispersionRepository.Setup(s => s.GetAllDefeatedAsync(It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(ExpiredData.GetInvoiceDispersionEmpty);
            _mockISystem.Setup(x => x.User).Returns(UserData.GetUser);
            _handler = new ExpiredCommandHandler(_mockICatalogBusiness.Object, _mockIUnitOfWork.Object, _mockIInvoiceDispersionRepository.Object, _mockINotificationRepository.Object, _mockISystem.Object);
            ExpiredCommand command = new ExpiredCommand();
            var result = await _handler.Handle(command, default);
            Assert.False(result.Value);
        }
    }
}