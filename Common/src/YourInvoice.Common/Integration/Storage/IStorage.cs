///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.Storage
{
    public interface IStorage
    {
        Task<object> UploadAsync(byte[] file, string path);

        Task<MemoryStream> DownloadAsync(string pathFile);

        Task<byte[]> DownloadByteAsync(string pathFile);

        Task<string> GenerateSecureDownloadUrlAsync(string url, string blobName, int tiempoExpiracionMinutos = 10);

        Task<bool> DeleteBlobByUrlAsync(string blobUrl);
    }
}