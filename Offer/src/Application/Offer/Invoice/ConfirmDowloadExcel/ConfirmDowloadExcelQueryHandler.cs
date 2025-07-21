///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.Offers;
using System.Data;
using System.Globalization;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Invoice.ConfirmDowloadExcel
{
    public sealed class ConfirmDowloadExcelQueryHandler : IRequestHandler<ConfirmDowloadExcelQuery, ErrorOr<byte[]>>
    {
        private readonly IOfferRepository repository;
        private const string numberInvoice = "No factura";
        private const string expirationDate = "Fecha de vencimiento";
        private const string datePay = "Fecha de pago";
        private const string invoiceValue = "Valor de la factura";
        private const string valueIva = "Valor Iva";
        private const string valuePay = "Valor neto de pago";
        private const string formatDate = "dd/MM/yyyy";
        private const string comment = "Ingrese [Fecha de pago] según el formato de fecha de su computadora: DD/MM/AAAA o MM/DD/AAAA";
        CultureInfo invariable = CultureInfo.InvariantCulture;
        private readonly ILogger<ConfirmDowloadExcelQueryHandler> _logger;

        public ConfirmDowloadExcelQueryHandler(IOfferRepository repository, ILogger<ConfirmDowloadExcelQueryHandler> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }

        public async Task<ErrorOr<byte[]>> Handle(ConfirmDowloadExcelQuery command, CancellationToken cancellationToken)
        {
            //si el estado de la oferta no es en progreso saca error
            if (!await this.repository.OfferIsInProgressAsync(command.offerId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            var result = await CreateFileExcelAsync(command.offerId);
            return result;
        }

        private async Task<byte[]> CreateFileExcelAsync(Guid offerId)
        {
            var dateToday = ExtensionFormat.DateTimeCOddmmyyyy();
            var invoices = await this.repository.GetInvoiceProcessConfirmAsync(offerId);
            var totalInvoice = invoices.Count;
            DataTable dt = new DataTable("ComfirmDataInvoice");
            dt.Columns.AddRange(new DataColumn[] { new DataColumn(numberInvoice), new DataColumn(expirationDate), new DataColumn(datePay, typeof(DateTime)), new DataColumn(invoiceValue), new DataColumn(valueIva), new DataColumn(valuePay) });

            invoices
           .OrderBy(o => o.Number)
           .ToList()
           .ForEach(inv => dt.Rows.Add(inv.Number, inv.DueDate?.ToString(formatDate, invariable)
                                      , inv.NegotiationDate?.ToString(formatDate, invariable), ((int)inv.Total).ToString("#,##0"),
                                      (inv.TaxAmount ?? 0).ToString("#,##0"), inv.NegotiationTotal > 0 ? ((int?)inv.NegotiationTotal) : ""));

            //_logger.LogInformation("       AQUI 1:            ");

            //foreach (DataRow row in dt.Rows)
            //{
            //    foreach (var item in row.ItemArray)
            //    {
            //        _logger.LogInformation(item + " - ");
            //    }                
            //}

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var sheet = workbook.AddWorksheet(dt, "Facturas");
                
                sheet.Protect().AllowElement(XLSheetProtectionElements.FormatCells) // Formato de celdas
                                .AllowElement(XLSheetProtectionElements.InsertColumns) // Inserción de columnas
                                .AllowElement(XLSheetProtectionElements.EditObjects) // Etidar
                                .AllowElement(XLSheetProtectionElements.DeleteColumns);// Eliminación de columnas

                sheet.Columns(1, 6).Width = 25;
                sheet.Column(3).Style.Protection.SetLocked(false);
                sheet.Column(6).Style.Protection.SetLocked(false);

                //_logger.LogInformation(" AQUI 2 FECHA CONTRA LA CUAL SE VA HA VALIDAR " + dateToday.Date.ToString());

                GetValidationExcel(totalInvoice, sheet, dateToday);

                //_logger.LogInformation("       AQUI 3:            ");
                //foreach (var row in sheet.RangeUsed().Rows())
                //{
                //    foreach (var cell in row.Cells())
                //    {
                //        _logger.LogInformation(cell.GetValue<string>() + " - ");
                //    }
                //}

                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        private static void GetValidationExcel(int totalInvoice, IXLWorksheet sheet, DateTime dateToday)
        {
            for (int i = 0; i < totalInvoice; i++)
            {
                int cn = i + 2;
                //sheet.Cell(cn, "C").Style.DateFormat.SetFormat("dd/MM/yyyy");
                sheet.Cell(cn, "C").CreateComment().AddText(comment).SetBold(true);
                sheet.Cell(cn, "C").GetComment().Style.Size.SetWidth(45).Size.SetHeight(30);
                sheet.Cell(cn, "C").GetDataValidation().Date.GreaterThan(dateToday.Date);
                sheet.Cell(cn, "C").GetDataValidation().ErrorStyle = XLErrorStyle.Stop;
                sheet.Cell(cn, "C").GetDataValidation().ErrorMessage = GetErrorDescription(MessageCodes.ValidationColumnExcelDate);

                sheet.Cell(cn, "F").Style.NumberFormat.Format = "#,##0";
                sheet.Cell(cn, "F").GetDataValidation().WholeNumber.GreaterThan(0);
                sheet.Cell(cn, "F").GetDataValidation().ErrorStyle = XLErrorStyle.Stop;
                sheet.Cell(cn, "F").GetDataValidation().ErrorMessage = GetErrorDescription(MessageCodes.ValidationColumnExcelNumeric);
            }
        }
    }
}