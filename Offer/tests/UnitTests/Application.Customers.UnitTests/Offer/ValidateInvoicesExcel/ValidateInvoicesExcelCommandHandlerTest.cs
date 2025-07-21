///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Files;
using yourInvoice.Offer.Application.Offer.Invoice.ValidateInvoicesExcel;
using yourInvoice.Offer.Domain.Invoices;
using System.Globalization;

namespace Application.Customer.UnitTest.Offer.ValidateInvoicesExcel
{
    public class ValidateInvoicesExcelCommandHandlerTest
    {
        private CultureInfo gobal = CultureInfo.CreateSpecificCulture("es-CO");

        [Fact]
        public async Task FileValid()
        {
            Guid offerId = Guid.NewGuid();

            var mockFileOperation = new Mock<IFileOperation>();
            mockFileOperation.Setup(x => x.ReadFileExcel<InvoiceExcelModel>(new byte[] { }))
                .Returns(new List<InvoiceExcelModel>()
                {
                    new InvoiceExcelModel()
                        { Fecha_de_pago = DateTime.Now.Add(TimeSpan.FromDays(2)).ToString("dd/MM/yyyy", gobal), No_factura = "FAC234", Valor_neto_de_pago = 1 }
                });

            var mockInvoiceRepository = new Mock<IInvoiceRepository>();
            mockInvoiceRepository.Setup(s => s.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            mockInvoiceRepository.Setup(x => x.GetAllByOffer(offerId)).ReturnsAsync(
                new List<yourInvoice.Offer.Domain.Invoices.Invoice>()
                {
                    new yourInvoice.Offer.Domain.Invoices.Invoice(new Guid(), offerId, "FAC234", "", "",
                        CatalogCode_InvoiceStatus.InProgress,
                        DateTime.Now, DateTime.Now, 1,1, new Guid(), 1, "", null, 1)
                }
            );

            var handler =
                new ValidateInvoicesExcelCommandHandler(mockInvoiceRepository.Object, mockFileOperation.Object, null);

            var request = new ValidateInvoicesExcelCommand(offerId, "");

            //act
            var result = await handler.Handle(request, CancellationToken.None);

            //expect
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task HasInvoicesWithNumbersInvalids()
        {
            Guid offerId = Guid.NewGuid();

            var mockFileOperation = new Mock<IFileOperation>();
            mockFileOperation.Setup(x => x.ReadFileExcel<InvoiceExcelModel>(new byte[] { }))
                .Returns(new List<InvoiceExcelModel>()
                {
                    new InvoiceExcelModel()
                        { Fecha_de_pago = DateTime.Now.Add(TimeSpan.FromDays(2)).ToString("dd/MM/yyyy",gobal), No_factura = "FAC235", Valor_neto_de_pago = 12200 }
                });

            var mockInvoiceRepository = new Mock<IInvoiceRepository>();
            mockInvoiceRepository.Setup(s => s.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            mockInvoiceRepository.Setup(x => x.GetAllByOffer(It.IsAny<Guid>())).ReturnsAsync(
                new List<yourInvoice.Offer.Domain.Invoices.Invoice>()
                {
                    new yourInvoice.Offer.Domain.Invoices.Invoice(new Guid(), offerId, "FAC234", "", "",
                        CatalogCode_InvoiceStatus.InProgress,
                        DateTime.Now, DateTime.Now, 1,1, new Guid(), 1, "", null, 1)
                }
            );

            var handler =
                new ValidateInvoicesExcelCommandHandler(mockInvoiceRepository.Object, mockFileOperation.Object, null);

            var request = new ValidateInvoicesExcelCommand(offerId, "");

            //act
            var result = await handler.Handle(request, CancellationToken.None);
            result.FirstError.Description.Should()
                .Be("El archivo ha sido rechazado, ya que contiene facturas no v�lidas en la oferta.");
        }

        [Fact]
        public async Task HasInvoicesWithDatesInvlids()
        {
            Guid offerId = Guid.NewGuid();

            var mockFileOperation = new Mock<IFileOperation>();
            mockFileOperation.Setup(x => x.ReadFileExcel<InvoiceExcelModel>(new byte[] { }))
                .Returns(new List<InvoiceExcelModel>()
                    { new InvoiceExcelModel() { Fecha_de_pago = DateTime.Now.ToString("dd/MM/yyyy",gobal), No_factura = "FAC234", Valor_neto_de_pago = 12200 } });

            var mockInvoiceRepository = new Mock<IInvoiceRepository>();
            mockInvoiceRepository.Setup(s => s.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            mockInvoiceRepository.Setup(x => x.GetAllByOffer(It.IsAny<Guid>())).ReturnsAsync(
                new List<yourInvoice.Offer.Domain.Invoices.Invoice>()
                {
                    new yourInvoice.Offer.Domain.Invoices.Invoice(new Guid(), offerId, "FAC234", "", "",
                        CatalogCode_InvoiceStatus.InProgress,
                        DateTime.Now, DateTime.Now, 1,1, new Guid(), 1, "", null, 1)
                }
            );

            var handler =
                new ValidateInvoicesExcelCommandHandler(mockInvoiceRepository.Object, mockFileOperation.Object, null);

            var request = new ValidateInvoicesExcelCommand(offerId, "");

            //act
            var result = await handler.Handle(request, CancellationToken.None);
            result.FirstError.Description.Should()
                .Be("El archivo ha sido rechazado, ya que contiene facturas con fechas menores o iguales a la de hoy.");
        }

        [Fact]
        public async Task HasDifferentInvoices()
        {
            Guid offerId = Guid.NewGuid();

            var mockFileOperation = new Mock<IFileOperation>();
            mockFileOperation.Setup(x => x.ReadFileExcel<InvoiceExcelModel>(new byte[] { }))
                .Returns(new List<InvoiceExcelModel>()
                {
                    new InvoiceExcelModel() { Fecha_de_pago = DateTime.Now.Add(TimeSpan.FromDays(2)).ToString("dd/MM/yyyy", gobal), No_factura = "FAC234", Valor_neto_de_pago = 12200 },
                    new InvoiceExcelModel() { Fecha_de_pago = DateTime.Now.Add(TimeSpan.FromDays(2)).ToString("dd/MM/yyyy", gobal), No_factura = "FAC235", Valor_neto_de_pago = 12200 }
                });

            var mockInvoiceRepository = new Mock<IInvoiceRepository>();
            mockInvoiceRepository.Setup(s => s.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            mockInvoiceRepository.Setup(x => x.GetAllByOffer(offerId)).ReturnsAsync(
                new List<yourInvoice.Offer.Domain.Invoices.Invoice>()
                {
                    new yourInvoice.Offer.Domain.Invoices.Invoice(new Guid(), offerId, "FAC234", "", "",
                        CatalogCode_InvoiceStatus.InProgress,
                        DateTime.Now, DateTime.Now.Add(TimeSpan.FromDays(2)), 1,1, new Guid(), 1, "", null, 1)
                }
            );

            var handler =
                new ValidateInvoicesExcelCommandHandler(mockInvoiceRepository.Object, mockFileOperation.Object, null);

            var request = new ValidateInvoicesExcelCommand(offerId, "");

            //act
            var result = await handler.Handle(request, CancellationToken.None);
            result.FirstError.Description.Should()
                .Be("El archivo ha sido rechazado, ya que no tiene la misma cantidad de facturas que tiene la oferta.");
        }

        [Fact]
        public async Task HasValuesLessThatZero()
        {
            Guid offerId = Guid.NewGuid();

            var mockFileOperation = new Mock<IFileOperation>();
            mockFileOperation.Setup(x => x.ReadFileExcel<InvoiceExcelModel>(new byte[] { }))
                .Returns(new List<InvoiceExcelModel>()
                {
                    new InvoiceExcelModel() { Fecha_de_pago = DateTime.Now.Add(TimeSpan.FromDays(2)).ToString("dd/MM/yyyy", gobal), No_factura = "FAC234", Valor_neto_de_pago = -1 },
                });

            var mockInvoiceRepository = new Mock<IInvoiceRepository>();
            mockInvoiceRepository.Setup(s => s.OfferIsInProgressAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            mockInvoiceRepository.Setup(x => x.GetAllByOffer(offerId)).ReturnsAsync(
                new List<yourInvoice.Offer.Domain.Invoices.Invoice>()
                {
                    new yourInvoice.Offer.Domain.Invoices.Invoice(new Guid(), offerId, "FAC234", "", "",
                        CatalogCode_InvoiceStatus.InProgress,
                        DateTime.Now, DateTime.Now.Add(TimeSpan.FromDays(2)), 1,1, new Guid(), 1, "", null, 1)
                }
            );

            var handler =
                new ValidateInvoicesExcelCommandHandler(mockInvoiceRepository.Object, mockFileOperation.Object, null);

            var request = new ValidateInvoicesExcelCommand(offerId, "");

            //act
            var result = await handler.Handle(request, CancellationToken.None);
            result.FirstError.Description.Should()
                .Be("El archivo ha sido rechazado, ya que contiene valores menores o iguales que cero.");
        }
    }
}