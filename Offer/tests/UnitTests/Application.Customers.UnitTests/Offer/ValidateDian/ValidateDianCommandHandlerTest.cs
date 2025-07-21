///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Bus;
using yourInvoice.Offer.Application.Offer.ValidateDian;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Offers.Queries;
using yourInvoice.Offer.Domain.Primitives;

namespace Application.Customer.UnitTest.Offer.ValidateDian
{
    public class ValidateDianCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldSendMessagesToQueue()
        {
            // Arrange
            Guid offerId = Guid.NewGuid();

            var _mockUnitOfWork = new Mock<IUnitOfWork>();

            var mockCatalogBusiness = new Mock<ICatalogBusiness>();
            mockCatalogBusiness.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new CatalogItemInfo { Name = "fakeConnectionString", Descripton = "fakeConnectionString" });

            var mockInvoiceRepository = new Mock<IInvoiceRepository>();
            mockInvoiceRepository.Setup(x => x.FindByStatus(offerId, CatalogCode_InvoiceStatus.Loaded))
                .ReturnsAsync(new List<yourInvoice.Offer.Domain.Invoices.Invoice> { new yourInvoice.Offer.Domain.Invoices.Invoice(new Guid(), offerId,"","","", CatalogCode_InvoiceStatus.Loaded,
                DateTime.Now,DateTime.Now,1,1,new Guid(),1,"",null,1) });

            var mockOfferRepository = new Mock<IOfferRepository>();
            mockOfferRepository.Setup(x => x.GetByIdWithNamesAsync(offerId))
                .ReturnsAsync(new GetOfferResponse("", "", "", "", Guid.NewGuid(), 1, 1, 1));

            mockOfferRepository.Setup(x => x.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var mockServiceBus = new Mock<IServiceBus>();

            var handler = new ValidateDianCommandHandler(mockInvoiceRepository.Object, _mockUnitOfWork.Object, mockServiceBus.Object, mockCatalogBusiness.Object, mockOfferRepository.Object);

            var request = new ValidateDianCommand(offerId);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Value);
            mockServiceBus.Verify(x => x.Start(It.IsAny<ServiceBusParameters>()), Times.Once);
            mockServiceBus.Verify(x => x.SendMessageAsync(It.IsAny<ValidateDianSend>(), It.IsAny<string>()), Times.Once);
            mockInvoiceRepository.Verify(x => x.Update(It.IsAny<yourInvoice.Offer.Domain.Invoices.Invoice>()), Times.Once);
        }
    }
}