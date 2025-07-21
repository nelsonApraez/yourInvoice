///*** projectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;

namespace yourInvoice.Common.UnitTest
{
    public class EmailBusinessUnitTest
    {
        private readonly Mock<ICatalogBusiness> mockICatalogRepository = new Mock<ICatalogBusiness>();

        [Fact]
        public void Email_ShouldSendOnEmailComponent()
        {
            // Act
            EmainBusiness email = new EmainBusiness(this.mockICatalogRepository.Object);
            email.Send("diego.moreno@projectCustom.com", "Email de prueba su factura", "<h1>prueba</h1>", "smtp.gmail.com", "587", "yourInvoicenotification@gmail.com", "ghkohgjmxfepfnbb", "yourInvoicenotificationFrom@gmail.com", "yourInvoicenotificationSender@gmail.com");

            // Assert
            Assert.NotEmpty("algo");
        }
    }
}