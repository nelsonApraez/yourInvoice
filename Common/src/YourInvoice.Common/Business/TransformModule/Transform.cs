///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using yourInvoice.Common.Entities;
using System.IO.Compression;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Common.Business.TransformModule
{
    public class TransformModule
    {
        public static string ReplaceTokens(string plantilla, Dictionary<string, string> datos)
        {
            string result = plantilla;
            foreach (var itemDato in datos)
            {
                result = result.Replace(itemDato.Key, itemDato.Value);
            }

            return result;
        }

        public static string GetFileExt(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public static Dictionary<string, byte[]> ExtractAndValidateZip(IFormFile zipFile, InvoiceProcessCache invoiceProcessCache)
        {
            var archivosExtraidos = new Dictionary<string, byte[]>();
            try
            {
                // Verifica si se ha proporcionado un archivo
                if (zipFile.Length == 0)
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileEmpty), zipFile.Name));
                    return archivosExtraidos;
                }

                using (var zipStream = zipFile.OpenReadStream())
                using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in zipArchive.Entries)
                    {
                        if (entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {   // Descomprime archivos .zip recursivamente
                            using (var tempMemoryStream = new MemoryStream())
                            {
                                entry.Open().CopyTo(tempMemoryStream); tempMemoryStream.Seek(0, SeekOrigin.Begin);
                                var archivosRecursivos = ExtractAndValidateZip(new FormFile(tempMemoryStream, 0, tempMemoryStream.Length, null, entry.Name), invoiceProcessCache);
                                // Agrega los archivos extraídos de la llamada recursiva a la lista principal
                                foreach (var archivoRecursivo in archivosRecursivos)
                                {
                                    archivosExtraidos.Add(archivoRecursivo.Key, archivoRecursivo.Value);
                                }
                            }
                        }
                        else if (entry.FullName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) || entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                entry.Open().CopyTo(memoryStream);
                                archivosExtraidos[entry.FullName] = memoryStream.ToArray();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.ErrorFile), zipFile.Name));
#if DEBUG
                Console.WriteLine($"Error al descomprimir o validar archivos: {ex.Message}");
#endif
            }
            return archivosExtraidos;
        }
    }
}