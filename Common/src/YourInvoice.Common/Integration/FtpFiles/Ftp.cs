///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using FluentFTP;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Entities;

namespace yourInvoice.Common.Integration.FtpFiles
{
    public class Ftp : IFtp, IDisposable
    {
        private readonly ICatalogBusiness catalogBusiness;
        private IEnumerable<CatalogItemInfo> ftpDataConection;
        private IEnumerable<CatalogItemInfo> ftpDataConectionFactoring;
        private FtpClient ftpClient;
        private FtpClient ftpClientFactoring;

        public Ftp(ICatalogBusiness catalogBusiness)
        {
            this.catalogBusiness = catalogBusiness;
        }

        public void Dispose()
        {
            // Liberar recursos no administrados
            ftpClient?.Dispose();
            ftpClientFactoring?.Dispose();
        }

        public async Task SendFileToFactoringAsync(byte[] fileBytes, string fileName)
        {
            try
            {
                var dataFtp = await GetDataConectionFTPFactoring();

                string host = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringHost).Descripton ?? string.Empty;
                string user = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringUser).Descripton ?? string.Empty;
                string path = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringPath).Descripton ?? string.Empty;
                string pasProjrd = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringPasProjrd).Descripton ?? string.Empty;

                using (FtpClient client = new FtpClient(host, user, pasProjrd, 990))
                {
                    //Se habilita seguridad
                    client.Config.EncryptionMode = FtpEncryptionMode.Implicit;
                    client.Config.ValidateAnyCertificate = true;
                    // Conectar al servidor FTP
                    client.Connect();

                    // Subir el archivo al servidor FTP
                    using (MemoryStream stream = new MemoryStream(fileBytes))
                    {
                        client.UploadStream(stream, path + fileName);
                    }
#if DEBUG
                    Console.WriteLine("Archivo subido exitosamente.");
#endif
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al subir el archivo al servidor FTP: {ex.Message}");
#endif
            }
        }

        private async Task<IEnumerable<CatalogItemInfo>> GetDataConectionFTPFactoring()
        {
            if (ftpDataConection is not null)
            {
                return ftpDataConection;
            }

            ftpDataConection = await this.catalogBusiness.ListByCatalogAsync(FtpConection.FtpFactoring);
            return ftpDataConection;
        }

        public async Task<bool> SendFileAsync(byte[] fileCsv, string nameFile)
        {
            try
            {
                var dataFtp = await GetDataConectionFTP();
                ftpClient = GetConectionFTP(dataFtp);
                var pathFile = dataFtp.First(c => c.Name == FtpConection.FtpPathFolderIN).Descripton;
                pathFile = pathFile + nameFile;
                using (MemoryStream stream = new MemoryStream(fileCsv))
                {
                    var resultFtp = ftpClient.UploadStream(stream, pathFile);
                    return resultFtp == FtpStatus.Success;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al subir el archivo: {ex.Message}");
#endif
                return false;
            }
            finally
            {
                if (ftpClient.IsConnected)
                {
                    ftpClient.Disconnect();
                }
            }
        }

        public async Task<byte[]> GetFileAsync(string nameFile, bool isDeleteFile)
        {
            try
            {
                var dataFtp = await GetDataConectionFTP();
                ftpClient = GetConectionFTP(dataFtp);
                var pathFile = dataFtp.First(c => c.Name == FtpConection.FtpPathFolderOUT).Descripton;
                pathFile = pathFile + nameFile;
                byte[] fileBytes;
                if (!ftpClient.FileExists(pathFile))
                {
                    return null;
                }
                ftpClient.DownloadBytes(out fileBytes, pathFile);
                if (isDeleteFile && fileBytes.Length > 0)
                {
                    ftpClient.DeleteFile(pathFile);
                }
                return fileBytes;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al descargar el archivo: {ex.Message}");
#endif
                return null;
            }
        }

        public async Task<List<string>> GetNameAllFilesDirectoryAsync()
        {
            try
            {
                var dataFtp = await GetDataConectionFactoringFTP();
                ftpClientFactoring = GetConectionFactoringFTP(dataFtp);
                var pathFile = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringPathOut).Descripton;
                var allFile = ftpClientFactoring.GetNameListing(pathFile).ToList();
                return allFile;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al descargar el archivo: {ex.Message}");
#endif
                return null;
            }
        }

        public async Task<List<string>> GetNameAllFilesDianFyMDirectoryAsync()
        {
            try
            {
                var dataFtp = await GetDataConectionFTP();
                ftpClient = GetConectionFTP(dataFtp);
                var pathFile = dataFtp.First(c => c.Name == FtpConection.FtpPathFolderOUT).Descripton;
                var allFile = ftpClient.GetNameListing(pathFile).ToList();
                return allFile;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al descargar el archivo: {ex.Message}");
#endif
                return new List<string>();
            }
        }

        public async Task<byte[]> GetFileFactoringAsync(string pathFile, bool isDeleteFile)
        {
            try
            {
                var dataFtp = await GetDataConectionFactoringFTP();
                ftpClientFactoring = GetConectionFactoringFTP(dataFtp);
                byte[] fileBytes;
                if (!ftpClientFactoring.FileExists(pathFile))
                {
                    return null;
                }
                ftpClientFactoring.DownloadBytes(out fileBytes, pathFile);
                if (isDeleteFile && fileBytes.Length > 0)
                {
                    ftpClientFactoring.DeleteFile(pathFile);
                }
                return fileBytes;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al descargar el archivo factoring: {ex.Message}");
#endif
                return null;
            }
        }

        private async Task<IEnumerable<CatalogItemInfo>> GetDataConectionFTP()
        {
            if (ftpDataConection is not null)
            {
                return ftpDataConection;
            }

            ftpDataConection = await this.catalogBusiness.ListByCatalogAsync(FtpConection.FtpFyM);
            return ftpDataConection;
        }

        private FtpClient GetConectionFTP(IEnumerable<CatalogItemInfo> ftpConecction)
        {
            if (ftpClient is null || !ftpClient.IsConnected)
            {
                string host = ftpConecction.First(c => c.Name == FtpConection.FtpHost).Descripton ?? string.Empty;
                string user = ftpConecction.First(c => c.Name == FtpConection.FtpUser).Descripton ?? string.Empty;
                string pasProjrd = ftpConecction.First(c => c.Name == FtpConection.FtpPasProjrd).Descripton ?? string.Empty;
                string port = ftpConecction.First(c => c.Name == FtpConection.FtpPort).Descripton ?? string.Empty;
                ftpClient = new FtpClient(host, user, pasProjrd, Convert.ToInt16(port));
                ftpClient.Connect();
            }

            return ftpClient;
        }

        private async Task<IEnumerable<CatalogItemInfo>> GetDataConectionFactoringFTP()
        {
            if (ftpDataConectionFactoring is not null)
            {
                return ftpDataConectionFactoring;
            }

            ftpDataConectionFactoring = await this.catalogBusiness.ListByCatalogAsync(FtpConection.FtpFactoring);
            return ftpDataConectionFactoring;
        }

        private FtpClient GetConectionFactoringFTP(IEnumerable<CatalogItemInfo> dataFtp)
        {
            if (ftpClientFactoring is null || !ftpClientFactoring.IsConnected)
            {
                string host = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringHost).Descripton ?? string.Empty;
                string user = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringUser).Descripton ?? string.Empty;
                string pasProjrd = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringPasProjrd).Descripton ?? string.Empty;
                string port = dataFtp.First(c => c.Id == CatalogCode_FTP_Factoring.FtpFactoringPort).Descripton ?? string.Empty;
                ftpClientFactoring = new FtpClient(host, user, pasProjrd, Convert.ToInt16(port));
                ftpClientFactoring.Config.EncryptionMode = FtpEncryptionMode.Implicit;
                ftpClientFactoring.Config.ValidateAnyCertificate = true;
                ftpClientFactoring.Connect();
            }
            return ftpClientFactoring;
        }
    }
}