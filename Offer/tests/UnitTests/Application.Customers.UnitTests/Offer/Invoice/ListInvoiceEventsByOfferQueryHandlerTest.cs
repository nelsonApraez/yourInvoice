///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Offer.Invoice.ListEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Invoices.Queries;

namespace Application.Customer.UnitTest.Offer.Invoice
{
    public class ListInvoiceEventsByOfferQueryHandlerTest
    {
        private readonly Mock<IInvoiceRepository> _mockRepository;
        private readonly ListInvoiceEventsByOfferQueryHandler _handler;

        public ListInvoiceEventsByOfferQueryHandlerTest()
        {
            _mockRepository = new Mock<IInvoiceRepository>();

            _handler = new ListInvoiceEventsByOfferQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task HandleListInvoiceEventsByOffer_WhenRepositoryReturnsData()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            SearchInfo searchInfo = new() { ColumnOrder = "InvoiceNumber", OrderType = "asc", PageSize = 3, StartIndex = 0 };

            ListInvoiceEventsByOfferQuery command = new(new Guid(), searchInfo);
            _mockRepository.Setup(x => x.ListEventsAsync(It.IsAny<Guid>(), searchInfo)).ReturnsAsync(new ListDataInfo<InvoiceListEventsResponse>());

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);
            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }
    }
}