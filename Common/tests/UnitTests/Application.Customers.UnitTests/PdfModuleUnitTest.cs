///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.Business.PdfModule;

namespace yourInvoice.Common.UnitTest
{
    public class PdfModuleUnitTest
    {
        [Fact]
        public void HtmlToPdf_ShouldConvertHtmlToPdf()
        {
            // Arrange
            string body = "<html>\r\n <body>\r\n  Hello World from selectpdf.com.\r\n </body>\r\n</html>";

            // Act
            MemoryStream result = PDFBusiness.HtmlToPdf(body);

            // Assert
            Assert.True(result.CanRead);
            Assert.True(result.CanSeek);
        }
    }
}