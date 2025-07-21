///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Offer.Invoice.Progress;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace Application.Customer.UnitTest.Offer.Invoice
{
    public class ProgressCommandHandlerTest
    {
        private readonly ProgressCommandHandler _handler;
        private IMemoryCache memoryCache;

        public ProgressCommandHandlerTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            memoryCache = serviceProvider.GetService<IMemoryCache>();

            _handler = new ProgressCommandHandler(memoryCache);
        }

        [Fact]
        public async Task HandleProgress_Sucess()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();
            memoryCache.Set(offerid, new InvoiceProcessCache(memoryCache, offerid));

            ProgressCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeFalse();
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HandleProgress_WhenOfferNotExist_ShouldReturnValidationError()
        {
            //Arrange
            // Se configura los parametros de entrada de nuestra prueba unitaria.
            Guid offerid = Guid.NewGuid();

            ProgressCommand command = new(offerid);

            //Act
            // Se ejecuta el metodo a probar de nuestra prueba unitaria
            var result = await _handler.Handle(command, default);

            //Assert
            // Se verifica los datos de retorno de nuestro metodo probado en la prueba unitaria
            result.IsError.Should().BeTrue();
            result.FirstError.Type.Should().Be(ErrorType.Validation);
            Assert.Equal(GetErrorDescription(MessageCodes.OfferNotExist), result.FirstError.Description);
        }

        [Fact]
        public void DeleteCache_ShouldRemoveFromMemoryCache()
        {
            // Arrange
            var cacheMock = new Mock<IMemoryCache>();
            var offerId = Guid.NewGuid();
            var invoiceProcessCache = new InvoiceProcessCache(cacheMock.Object, offerId);

            // Act
            invoiceProcessCache.DeleteCache();

            // Assert
            cacheMock.Verify(cache => cache.Remove(offerId), Times.Once);
        }
    }
}