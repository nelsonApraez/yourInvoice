///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.Files
{
    public interface IFileOperation
    {
        byte[] CreateFileCsv<T>(List<T> data) where T : class;

        List<T> ReadFileCsv<T>(byte[] data) where T : class;

        List<T> ReadFileExcel<T>(byte[] data) where T : class;

        byte[] CreateFileExcelAsync<T>(List<T> items, string sheetName) where T : class;
    }
}