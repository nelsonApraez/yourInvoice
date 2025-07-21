///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using MediatR;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Offer.Application.Admin.EmailToSeller;

namespace Application.Customer.UnitTest.Admin.EmailToSeller
{
    public class EmailToSellerAdminPurchasedCommandHanlerTest
    {
        private readonly Mock<ICatalogBusiness> _mockICatalogBusiness;
        private readonly Mock<IMediator> _mockIMediator;
        private readonly Mock<IEventNotificationRepository> _mockeventNotificationRepository;
        private readonly Mock<yourInvoice.Offer.Domain.Offers.IOfferRepository> _mockofferRepository;
        private readonly Mock<IUnitOfWorkCommonEF> _mockunitOfWork;

        private EmailToSellerAdminPurchasedCommandHanler _handler;

        public EmailToSellerAdminPurchasedCommandHanlerTest()
        {
            _mockICatalogBusiness = new Mock<ICatalogBusiness>();
            _mockIMediator = new Mock<IMediator>();
            _mockeventNotificationRepository = new Mock<IEventNotificationRepository>();
            _mockofferRepository = new Mock<yourInvoice.Offer.Domain.Offers.IOfferRepository>();
            _mockunitOfWork = new Mock<IUnitOfWorkCommonEF>();
        }

        [Fact]
        public async Task HandEmailToSellerAdmin_WhenNotIs_empty()
        {
            var result = true;
            var command = new EmailToSellerAdminPurchasedCommand()
            {
                EmailsSeller = new List<string>(),
                AttachData = new Dictionary<string, string>(),
                AttachFilesData = new List<yourInvoice.Common.Entities.AttachFile>() { new yourInvoice.Common.Entities.AttachFile { File = EmailToSellerAdminPurchasedData.GetStream } }
            };

            _mockICatalogBusiness.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(EmailToSellerAdminPurchasedData.GetCatalogItemInfo);

            _handler = new EmailToSellerAdminPurchasedCommandHanler(_mockICatalogBusiness.Object, _mockeventNotificationRepository.Object, _mockofferRepository.Object, _mockunitOfWork.Object);

            await _handler.Handle(command, default);

            Assert.True(result);
        }
    }
}