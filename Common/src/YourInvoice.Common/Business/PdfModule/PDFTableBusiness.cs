///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas;
using iText.Html2pdf;
using iText.IO.Font.Constants;
using iText.IO.Image;
using Microsoft.IdentityModel.Tokens;

namespace yourInvoice.Common.Business.PdfModule
{
    public class PDFTableBusiness
    {
        public static MemoryStream TableToPdf(Table dataLinking, Table dataSignature, PageSize pageSize, PdfFont font, string watermarkText, bool isNatural)
        {
            var memoryStream = new MemoryStream();

            using (var pdfWriter = new PdfWriter(memoryStream))
            using (var pdfDocument = new PdfDocument(pdfWriter))
            using (var document = new Document(pdfDocument, pageSize))
            {
                document.SetMargins(10, 10, 10, 10);
                document.Add(dataLinking);
                //Se agrega nueva hoja para que contenga la información de declaraciones y firmas.
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                document.Add(dataSignature);
                //Se agrega marca de agua
                AddWatermark(pdfDocument, font, watermarkText, isNatural);
            }
            return memoryStream;
        }

        public static Cell SetCell(int typeCell, int row, int col, PdfFont font, bool isLeft = true, bool isRigth = true)
        {
            var border = new SolidBorder(new DeviceRgb(0, 74, 160), 1f);
            var backgroundTitle = new DeviceRgb(230, 237, 245);
            var fontColorTitle = new DeviceRgb(0, 74, 160);
            //Tipo de celda que se esta configurando
            switch (typeCell)
            {
                case 0: //Header
                    return new Cell(row, col)
                        .SetFont(font)
                        .SetFontSize(16f)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                        .SetPadding(4f)
                        .SetMinHeight(50f)
                        .SetBorderTop(border)
                        .SetBorderRight(isRigth ? border : Border.NO_BORDER)
                        .SetBorderBottom(border)
                        .SetBorderLeft(isLeft ? border : Border.NO_BORDER);
                case 1: //Title
                    return new Cell(row, col)
                        .SetBackgroundColor(backgroundTitle)
                        .SetFont(font)
                        .SetFontColor(fontColorTitle)
                        .SetFontSize(10f)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(0.25f)
                        .SetMarginBottom(0.25f)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderRight(border)
                        .SetBorderBottom(border)
                        .SetBorderLeft(border);
                case 2: //Label
                    return new Cell(row, col)
                        .SetBackgroundColor(backgroundTitle)
                        .SetFont(font)
                        .SetFontColor(fontColorTitle)
                        .SetFontSize(8f)
                        .SetPaddings(0f, 5f, 0f, 5f)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderRight(isRigth ? border : Border.NO_BORDER)
                        .SetBorderBottom(border)
                        .SetBorderLeft(isLeft ? border : Border.NO_BORDER);
                case 3: //Field
                    return new Cell(row, col)
                        .SetFont(font)
                        .SetFontSize(10f)
                        .SetMinHeight(16f)
                        .SetPaddings(0f, 5f, 0f, 5f)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderRight(isRigth ? border : Border.NO_BORDER)
                        .SetBorderBottom(border)
                        .SetBorderLeft(isLeft ? border : Border.NO_BORDER);
                case 4: //Field only borders
                    return new Cell(row, col)
                        .SetFont(font)
                        .SetFontSize(10f)
                        .SetPaddingTop(0f)
                        .SetPaddingBottom(0f)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderRight(isRigth ? border : Border.NO_BORDER)
                        .SetBorderBottom(border)
                        .SetBorderLeft(isLeft ? border : Border.NO_BORDER);
                case 5: //Field signature
                    return new Cell(row, col)
                        .SetFont(font)
                        .SetFontSize(8f)
                        .SetBorderTop(Border.NO_BORDER)
                        .SetBorderRight(isRigth ? border : Border.NO_BORDER)
                        .SetBorderBottom(border)
                        .SetBorderLeft(isLeft ? border : Border.NO_BORDER);
                default:
                    return new Cell();
            }
        }

        private static void AddWatermark(PdfDocument pdfDoc, PdfFont font, string watermarkText, bool isNatural)
        {
            var numberOfPages = pdfDoc.GetNumberOfPages();

            bool notApply = isNatural ? (numberOfPages <= 3) : (numberOfPages <= 4);

            if (notApply)
                return;

            PdfPage page = pdfDoc.GetPage(numberOfPages - 1);
            Rectangle pageSize = page.GetPageSize();

            double rotationInRads = 0; //Grados de inclinación.

            float fontSize = 30;

            // Crear PdfCanvas para la penúltima página
            PdfCanvas canvas = new PdfCanvas(page);

            // Dividir el texto en líneas
            string[] lines = watermarkText.Split('\n');

            // Aplicar transformaciones: rotación y traducción
            canvas.SaveState();

            //Obtener dimensiones del texto para centrarlo
            float textHeight = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                float textWidth = Math.Max(0, font.GetWidth(line, fontSize));
                textHeight += font.GetAscent(line, fontSize) - font.GetDescent(line, fontSize);
                //Validación si es el primer texto
                bool isFirst = (textHeight - (font.GetAscent(line, fontSize) - font.GetDescent(line, fontSize))) == 0;
                //Posición x,y donde se va a ubicar el texto.
                float bottomLeftX = pageSize.GetWidth() / 2 - (textWidth / 2);
                float bottomLeftY = pageSize.GetHeight() / 2 - (textHeight / 2) - (isFirst ? 0 : fontSize * i - 1);

                if (!isNatural && numberOfPages <= 4)
                    bottomLeftY += 80;

                canvas.BeginText()
                      .SetColor(ColorConstants.GRAY, true)
                      .SetExtGState(new PdfExtGState().SetFillOpacity(0.1f))
                      .SetFontAndSize(font, fontSize)
                      .SetTextMatrix(
                          (float)Math.Cos(rotationInRads),
                          (float)-Math.Sin(rotationInRads),
                          (float)Math.Sin(rotationInRads),
                          (float)Math.Cos(rotationInRads),
                          bottomLeftX,
                          bottomLeftY
                      )
                      .ShowText(line)
                      .EndText();
            }

            canvas.RestoreState();

        }

        public static MemoryStream HtmlToPdf(string html, PageSize pageSize, string headerImagePath = null, bool isAuth = false, bool isVinculation = true)
        {
            byte[] pdfInBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var pdfWriter = new PdfWriter(memoryStream))
                {
                    var pdfDocument = new PdfDocument(pdfWriter);
                    var document = new Document(pdfDocument);
                    
                    HtmlConverter.ConvertToPdf(html, pdfDocument, null);
                }
                pdfInBytes = memoryStream.ToArray();
            }

            using (var inputMemoryStream = new MemoryStream(pdfInBytes))
            {
                inputMemoryStream.Position = 0;
                using (var pdfReader = new PdfReader(inputMemoryStream))
                {
                    using (var outputMemoryStream = new MemoryStream())
                    {
                        using (var pdfWriter = new PdfWriter(outputMemoryStream))
                        {
                            using (var pdfDocument = new PdfDocument(pdfReader, pdfWriter))
                            {
                                
                                if (isVinculation)
                                {
                                    if (!headerImagePath.IsNullOrEmpty())
                                        AddHeaderImage(pdfDocument, headerImagePath);

                                    AddFooterWithPagination(pdfDocument, isAuth);
                                }
                            }
                        }
                        return outputMemoryStream;
                    }
                }
            }
        }

        private static void AddHeaderImage(PdfDocument pdfDocument, string headerImagePath)
        {
            var numberOfPages = pdfDocument.GetNumberOfPages();
            for (int i = 1; i <= numberOfPages; i++)
            {
                var page = pdfDocument.GetPage(i);
                var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDocument);

                var imageData = ImageDataFactory.Create(headerImagePath);
                var image = new Image(imageData)
                    .SetFixedPosition(i, page.GetPageSize().GetWidth() - 200, page.GetPageSize().GetHeight() - 50)
                    .SetHeight(25f);

                var document = new Document(pdfDocument);
                document.Add(image);
            }
        }

        private static void AddFooterWithPagination(PdfDocument pdfDocument, bool isAuth)
        {
            var numberOfPages = pdfDocument.GetNumberOfPages();

            var x = isAuth ? 560 : 290;
            var y = 30;
            for (int i = 1; i <= numberOfPages; i++)
            {
                var page = pdfDocument.GetPage(i);
                var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDocument);

                canvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN), 8)
                    .MoveText(x, y)
                    .ShowText(isAuth ? $"{i}" : $"PÁGINA {i} DE {numberOfPages}")
                    .EndText();
            }
        }


        public static void TableToPdfTest(Table dataLinking, Table dataSignature, PageSize pageSize, PdfFont font, string watermarkText, bool isNatural)
        {
            string outputPath = "Assets/Formato de vinculación.pdf";

            using (var pdfWriter = new PdfWriter(outputPath))
            using (var pdfDocument = new PdfDocument(pdfWriter))
            using (var document = new Document(pdfDocument, pageSize))
            {
                document.SetMargins(10, 10, 10, 10);

                document.Add(dataLinking);

                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                document.Add(dataSignature);

                // Ahora que hemos agregado el contenido, procedemos a agregar la marca de agua
                AddWatermark(pdfDocument, font, watermarkText, isNatural);
            }
        }

        public static void HtmlToPdfTest(string html, PageSize pageSize, string headerImagePath = null, bool isAuth = false, bool isVinculation = true)
        {
            string outputPath = "Assets/Documento.pdf";

            byte[] pdfInBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var pdfWriter = new PdfWriter(memoryStream))
                {
                    var pdfDocument = new PdfDocument(pdfWriter);
                    var document = new Document(pdfDocument);

                    HtmlConverter.ConvertToPdf(html, pdfDocument, null);
                }
                pdfInBytes = memoryStream.ToArray();
            }

            using (var inputMemoryStream = new MemoryStream(pdfInBytes))
            {
                inputMemoryStream.Position = 0;
                using (var pdfReader = new PdfReader(inputMemoryStream))
                {

                    using (var pdfWriter = new PdfWriter(outputPath))
                    {
                        using (var pdfDocument = new PdfDocument(pdfReader, pdfWriter))
                        {
                            if (isVinculation)
                            {
                                if (!headerImagePath.IsNullOrEmpty())
                                    AddHeaderImage(pdfDocument, headerImagePath);

                                AddFooterWithPagination(pdfDocument, isAuth);
                            }
                        }
                    }
                }
            }
        }
    }
}
