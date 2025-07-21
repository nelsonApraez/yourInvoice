///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ClosedXML.Excel;
using System.Data;

namespace yourInvoice.Common.Business.ExcelModule
{
    public static class ExcelBusiness
    {
        private static readonly object lockObject = new object();

        public static MemoryStream Generate(DataTable dataTable, bool setShowHeaderRow = true, bool setShowRowStripes = false, bool setShowColumnStripes = false, bool setShowAutoFilter = false, bool setShowTotalsRow = false)
        {
            using var wb = new XLWorkbook();
            var ws = wb.AddWorksheet();
            ws.Cell("A1").InsertTable(dataTable)
                .SetShowHeaderRow(setShowHeaderRow)
                .SetShowRowStripes(setShowRowStripes)
                .SetShowColumnStripes(setShowColumnStripes)
                .SetShowAutoFilter(setShowAutoFilter)
                .SetShowTotalsRow(false);

            var streamExcelToReturn = new MemoryStream();
            using (wb)
            using (var streamExcel = new MemoryStream())
            {
                lock (lockObject)
                {
                    wb.SaveAs(streamExcel);
                    streamExcel.Position = 0;
                    streamExcel.CopyTo(streamExcelToReturn);
                }
            }

            return streamExcelToReturn;
        }
    }
}