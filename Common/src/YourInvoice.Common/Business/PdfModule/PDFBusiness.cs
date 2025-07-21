///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using SelectPdf;

namespace yourInvoice.Common.Business.PdfModule
{
    public class PDFBusiness
    {
        /// <summary>
        /// Metodo para convertir un HTML a PDF.
        /// Documentación: https://selectpdf.com/demo/convert-html-code-to-pdf.aspx
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageSize"></param>
        /// <param name="pdfOrientation"></param>
        /// <param name="webPageWidth"></param>
        /// <param name="webPageHeight"></param>
        /// <returns></returns>
        public static MemoryStream HtmlToPdf(string html, PdfPageSize pageSize = PdfPageSize.Letter,
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait, int webPageWidth = 640,
            int webPageHeight = 0, int MarginTop = 32, int MarginRight = 12, int MarginLeft = 0, int MarginBottom = 24)
        {
            // read parameters from the webpage
            string htmlString = html;
            string baseUrl = null;

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginTop = MarginTop;
            converter.Options.MarginRight = MarginRight;
            converter.Options.MarginLeft = MarginLeft;
            converter.Options.MarginBottom = MarginBottom;

            converter.Options.RenderingEngine = RenderingEngine.WebKit;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);

            // save pdf document
            var streamPdf = new MemoryStream(doc.Save());

            // close pdf document
            //streamPdf.Close();
            doc.Close();

            return streamPdf;
        }
    }
}