///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.FtpFiles
{
    public interface IFtp
    {
        Task<bool> SendFileAsync(byte[] fileCsv, string nameFile);

        Task<byte[]> GetFileAsync(string nameFile, bool isDeleteFile);

        Task SendFileToFactoringAsync(byte[] fileBytes, string fileName);

        Task<List<string>> GetNameAllFilesDirectoryAsync();

        Task<byte[]> GetFileFactoringAsync(string pathFile, bool isDeleteFile);

        Task<List<string>> GetNameAllFilesDianFyMDirectoryAsync();
    }
}