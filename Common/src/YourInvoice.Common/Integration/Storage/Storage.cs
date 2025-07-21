///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using yourInvoice.Common.Business.CatalogModule;

namespace yourInvoice.Common.Integration.Storage
{
    public class Storage : IStorage
    {
        private const string resourceIsBlob = "b";
        private const string connectionStringAccountKey = "AccountKey";

        private readonly ICatalogBusiness _catalogBusiness;

        public Storage(ICatalogBusiness catalogBusiness)
        {
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
        }

        public async Task<object> UploadAsync(byte[] file, string path)
        {
            //obtener parametros desde los catalogos y validamos que la descripción tenga un valor
            var connectionString = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ConexionString);
            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString.Descripton);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName.Descripton);
            BlobClient blobClient = containerClient.GetBlobClient(path);

            BlobUploadOptions options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "application/octet-stream" } // Puedes ajustar el tipo de contenido según tu archivo
            };

            try
            {
                // Crear un MemoryStream a partir del byte[]
                using (MemoryStream memoryStream = new MemoryStream(file))
                {
                    await blobClient.UploadAsync(memoryStream, options);
                }

                return blobClient.Uri;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task<MemoryStream> DownloadAsync(string pathFile)
        {
            var connectionString = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ConexionString);
            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString.Descripton);
            var exits = await blobServiceClient.GetBlobContainerClient(containerName.Descripton).GetBlobClient(pathFile).ExistsAsync();
            if (!exits)
            {
#if DEBUG
                Console.WriteLine($"No existe el archivo {pathFile} en el BlobStorage");
#endif
                return new MemoryStream();
            }
            BlobClient blobClient = blobServiceClient.GetBlobContainerClient(containerName.Descripton).GetBlobClient(pathFile);
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);
                return memoryStream;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error la descargar contenido del blobstorage: {ex.Message}");
#endif
                return new MemoryStream();
            }
        }

        public async Task<byte[]> DownloadByteAsync(string pathFile)
        {
            var result = await DownloadAsync(pathFile);
            return result.ToArray();
        }

        public async Task<string> GenerateSecureDownloadUrlAsync(string url, string blobName, int tiempoExpiracionMinutos = 10)
        {
            try
            {
                var connectionString = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ConexionString);
                var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

                string urlGenerated = url;
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString.Descripton);
                string pathBlob = url.Replace(blobServiceClient.Uri.ToString() + containerName.Descripton.Trim(), "");
                BlobClient blobClient = blobServiceClient.GetBlobContainerClient(containerName.Descripton).GetBlobClient(pathBlob);
                var accountKey = await GetAcountKey();
                if (blobClient.CanGenerateSasUri)
                {
                    BlobSasBuilder sasBuilder = new BlobSasBuilder
                    {
                        BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                        BlobName = blobClient.Name,
                        Resource = resourceIsBlob,
                        StartsOn = DateTimeOffset.UtcNow,
                        ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(tiempoExpiracionMinutos),
                        Protocol = SasProtocol.Https
                    };
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
                    string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(blobClient.AccountName, accountKey)).ToString();
                    string urlToken = $"{blobClient.Uri}?{sasToken}";
                    urlGenerated = urlToken;
                }
                return await Task.Run(() =>
                {
                    return urlGenerated;
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al generar la URL de descarga segura: {ex.Message}");
#endif
                return null;
            }
        }

        private async Task<string> GetAcountKey()
        {
            var connectionString = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ConexionString);

            var temp = connectionString.Descripton.Split(';')
                                        .Select(s => s.Split(new char[] { '=' }, 2));

            var valueConnetion = temp.Where(x => x.Count() > 1).ToDictionary(s => s[0], s => s[1]);

            string key = string.Empty;
            if (!valueConnetion.TryGetValue(connectionStringAccountKey, out key))
            {
                return key;
            }
            return key;
        }

        public async Task<bool> DeleteBlobByUrlAsync(string blobUrl)
        {
            try
            {
                var connectionString = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ConexionString);
                var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString.Descripton);
                BlobContainerClient cont = blobServiceClient.GetBlobContainerClient(containerName.Descripton);

                string blobName = GetBlobNameFromUrl(blobUrl, containerName.Descripton);

                cont.GetBlobClient(blobName).DeleteIfExists();

                return true; // Si no se lanza ninguna excepción, consideramos que la eliminación fue exitosa
            }
            catch (Exception)
            {
                // Manejar cualquier excepción y retornar false si la eliminación no fue exitosa
                return false;
            }
        }

        private static string GetBlobNameFromUrl(string url, string container)
        {
            // Busca la posición de la subcadena en el texto
            int indice = url.IndexOf(container);

            // Si la subcadena no se encuentra, devuelve el texto completo
            if (indice == -1)
            {
                return url;
            }

            // Utiliza Substring para obtener la parte derecha basada en la posición de la subcadena
            return url.Substring(indice + container.Length);
        }
    }
}