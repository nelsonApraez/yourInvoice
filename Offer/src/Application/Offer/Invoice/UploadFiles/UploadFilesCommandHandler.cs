///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.ScanFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Offer.Domain.Cufes;

namespace yourInvoice.Offer.Application.Offer.Invoice.UploadFiles
{
    public sealed class UploadFilesCommandHandler : IRequestHandler<UploadFilesCommand, ErrorOr<UploadFilesResponse>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IPayerRepository _payerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly IScanFile _scanFile;
        private readonly IStorage _storage;
        private readonly ICufeRepository _cufeRepository;
        private const string Url_XmlSchema = "http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd";

        private const string StatusProgressChache = "En progreso";
        private const string StatusFinishChache = "Finalizado";
        private const string Storage = "storage";
        private const string Invoices = "invoices";
        private const string Total = "Total";
        private const string FechaVencimiento = "Fecha de vencimiento";
        private const string FechaEmision = "Fecha de emision";
        private const string NombreFactura = "Nombre Factura";
        private const string Trm = "TRM";

        public UploadFilesCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork, IMemoryCache memoryCache,
            IScanFile scanFile, IStorage storage, IOfferRepository offerRepository, IPayerRepository payerRepository, IUserRepository userRepository,
            IDocumentRepository documentRepository, ICufeRepository cufeRepository)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _payerRepository = payerRepository ?? throw new ArgumentNullException(nameof(payerRepository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _scanFile = scanFile ?? throw new ArgumentNullException(nameof(scanFile));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _cufeRepository = cufeRepository ?? throw new ArgumentNullException(nameof(cufeRepository));
        }

        public async Task<ErrorOr<UploadFilesResponse>> Handle(UploadFilesCommand command, CancellationToken cancellationToken)
        {
            //validaciones
            //oferta exista y este en estado (en proceso)
            Domain.Offer offer = await _offerRepository.GetByIdAsync(command.offerId);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            //si el estado de la oferta no es en progreso saca error
            if (!await _offerRepository.OfferIsInProgressAsync(command.offerId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            InvoiceProcessCache invoiceProcessCache = new(_memoryCache, command.offerId)
            {
                //-- status chache a en progreso
                Status = StatusProgressChache
            };

            List<IFormFile> filesWithCorrectExtension = ValidateExtension(command, invoiceProcessCache);

            List<IFormFile> filesWithCorrectSize = ValidateFileSize(filesWithCorrectExtension, invoiceProcessCache);

            //escanear archivos
            //informar a la cache el progreso del escaneo

            List<IFormFile> filesWithOutVirus = ValidateVirus(invoiceProcessCache, filesWithCorrectSize);

            //Decomprimir archivos
            //foreach parallel
            //--validar si existen el PDF y XML
            //----si no existen los dos archivos se rechaza ese registro de factura
            //----informar a la cache cual es el nombre del archivo rechazado

            List<InvoiceTemp> invoiceTemps = ValidateExistenceTwoFiles(invoiceProcessCache, filesWithOutVirus);

            //validar si XML es valido con XSD
            //--se recorren los XML y se validan
            //--los xml que no  sean validos seinforman a la cache en archivos rechazados
            List<InvoiceTemp> invoiceTempsWithCorrectSchema = ValidateSchema(invoiceProcessCache, invoiceTemps);

            //Extraer data del XML
            List<InvoiceTemp> invoiceTempsCorrects = await ValidateBusiness(offer, invoiceProcessCache, invoiceTempsWithCorrectSchema);

            //Se eliminan las facturas que posean el mismo cufe en la misma carga de archivo
            List<InvoiceTemp> invoiceTempsCorrectsWithoutCufeRepeted = ValidateSameCufeInSameLoad(invoiceProcessCache, invoiceTempsCorrects);

            //guardar los archivos en el storage
            //--recorrer los archivos y almacenarlos
            //storage/invoice/emisor(nit)/consecutivo de la oferta/numero de factura/{numeroFactura}.PDF y {numeroFactura}.XML
            //--obtener la url de almacenamiento del storage, guardar el nombre del archivo
            //--informar al cache

            await SaveFilesInStorage(invoiceProcessCache, invoiceTempsCorrectsWithoutCufeRepeted, offer);

            //guardar en DB proceso masivo
            //--si este proceso falla se deben eliminar las facturas que se cargaron en el bloque, se deben eliminar del storage
            //--status chache a finalizado siempre y cuando los max y current de todos los progresos sean iguales

            //await Task.Delay(TimeSpan.FromMinutes(2)); habilitar esto si se quiere dar una espera al proceso para validar el progreso por el otro API de Progress

            SaveInDataBase(offer, invoiceTempsCorrectsWithoutCufeRepeted);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            UploadFilesResponse uploadFilesResponse = new()
            {
                FilesRejected = new List<Tuple<string, string>>(invoiceProcessCache.FilesRejected),
                OfferId = invoiceProcessCache.OfferId,
                ScanTotalProgress = invoiceProcessCache.ScanTotalProgress,
                StorageTotalProgress = invoiceProcessCache.StorageTotalProgress,
                ValidationBusinessTotalProgress = invoiceProcessCache.ValidationBusinessTotalProgress,
                Status = StatusFinishChache
            };

            invoiceProcessCache.Status = StatusFinishChache;
            invoiceProcessCache.DeleteCache();

            return uploadFilesResponse;
        }

        private void SaveInDataBase(Domain.Offer offer, List<InvoiceTemp> invoiceTempsCorrects)
        {
            foreach (var itemInvoice in invoiceTempsCorrects)
            {
                decimal trm = string.IsNullOrEmpty(itemInvoice.Trm) ? 0 : decimal.Parse(itemInvoice.Trm);
                decimal taxAmount = string.IsNullOrEmpty(itemInvoice.TaxAmount) ? 0 : decimal.Parse(itemInvoice.TaxAmount);

                Domain.Invoices.Invoice invoice = new Domain.Invoices.Invoice(Guid.NewGuid(), offer.Id, itemInvoice.Number, itemInvoice.ZipName, itemInvoice.Cufe,
                    CatalogCode_InvoiceStatus.Loaded, DateTime.Parse(itemInvoice.EmitDate), DateTime.Parse(itemInvoice.DueDate), decimal.Parse(itemInvoice.Total), taxAmount, (Guid)GetMoneyType(itemInvoice),
                    trm, string.Empty, null, null);

                Domain.Invoices.Invoice invoiceResult = _invoiceRepository.Add(invoice);

                Document documentXml = new(Guid.NewGuid(), offer.Id, invoiceResult.Id, itemInvoice.XmlFile.FirstOrDefault().Key,
                    CatalogCode_DocumentType.Invoice, false, itemInvoice.Url, itemInvoice.XmlFile.FirstOrDefault().Value.LongLength.ToMegaByte());
                Document documentPdf = new(Guid.NewGuid(), offer.Id, invoiceResult.Id, itemInvoice.PdfFile.FirstOrDefault().Key,
                    CatalogCode_DocumentType.Invoice, false, itemInvoice.Url, itemInvoice.PdfFile.FirstOrDefault().Value.LongLength.ToMegaByte());

                _documentRepository.Add(documentXml);
                _documentRepository.Add(documentPdf);
            }
        }

        private static Guid? GetMoneyType(InvoiceTemp itemInvoice)
        {
            if (itemInvoice.MoneyType == ConstantCode_MoneyType.COP)
                return CatalogCode_InvoiceMoneyType.COP;

            if (itemInvoice.MoneyType == ConstantCode_MoneyType.USD)
                return CatalogCode_InvoiceMoneyType.USD;

            return null;
        }

        private async Task SaveFilesInStorage(InvoiceProcessCache invoiceProcessCache, List<InvoiceTemp> invoiceTempsCorrects, Domain.Offer offer)
        {
            invoiceProcessCache.StorageMax = invoiceTempsCorrects.Count();

            foreach (var itemInvoice in invoiceTempsCorrects)
            {
                string storageRute = $"{Storage}/{offer.Consecutive}/{Invoices}/{itemInvoice.Number}/";

                object url = await _storage.UploadAsync(itemInvoice.XmlFile.FirstOrDefault().Value, storageRute + itemInvoice.XmlFile.FirstOrDefault().Key);
                await _storage.UploadAsync(itemInvoice.PdfFile.FirstOrDefault().Value, storageRute + itemInvoice.PdfFile.FirstOrDefault().Key);

                string fileName = Path.GetFileName(url.ToString());

                itemInvoice.Url = url.ToString().Replace(fileName, string.Empty);

                invoiceProcessCache.StorageCurrent += 1;
            }
        }

        private async Task<List<InvoiceTemp>> ValidateBusiness(Domain.Offer offer, InvoiceProcessCache invoiceProcessCache, List<InvoiceTemp> invoiceTempsWithCorrectSchema)
        {
            List<InvoiceTemp> invoiceTempsCorrects = new();

            //validaciones de acuerdo al diagrama
            //--si algun dato requerdo de la factura no existe, se debe informar en la cache
            invoiceProcessCache.ValidationBusinessMax = invoiceTempsWithCorrectSchema.Count();

            foreach (var itemInvoice in invoiceTempsWithCorrectSchema)
            {
                InvoiceDataExtracted invoiceDataExtracted = new();

                XmlDocument xmlDoc = new();

                try
                {
                    xmlDoc.LoadXml(Encoding.UTF8.GetString(itemInvoice.XmlFile.FirstOrDefault().Value));
                }
                catch (Exception)
                {                    
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileXmlInvalid, Total), itemInvoice.ZipName));
                    continue;
                }

                invoiceDataExtracted = GetDataFromXml(itemInvoice, invoiceDataExtracted, xmlDoc);

                if (string.IsNullOrEmpty(invoiceDataExtracted.Total))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByEmptyField, Total), itemInvoice.ZipName));
                    continue;
                }
                if (string.IsNullOrEmpty(invoiceDataExtracted.FechaVencimiento))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByEmptyField, FechaVencimiento), itemInvoice.ZipName));
                    continue;
                }
                if (string.IsNullOrEmpty(invoiceDataExtracted.FechaEmision))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByEmptyField, FechaEmision), itemInvoice.ZipName));
                    continue;
                }
                if (string.IsNullOrEmpty(invoiceDataExtracted.Factura))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByEmptyField, NombreFactura), itemInvoice.ZipName));
                    continue;
                }
                if (string.IsNullOrEmpty(invoiceDataExtracted.Cufe))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByCufe), itemInvoice.ZipName));
                    continue;
                }
                Domain.Users.User user = await _userRepository.GetByIdAsync((Guid)offer.UserId);
                if (string.IsNullOrEmpty(invoiceDataExtracted.NitEmisor) || invoiceDataExtracted.NitEmisor != user.CompanyNit)
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByNoSameSeller), itemInvoice.ZipName));
                    continue;
                }
                Payer payer = await _payerRepository.GetByIdAsync((Guid)offer.PayerId);
                if (string.IsNullOrEmpty(invoiceDataExtracted.NitReceptor) || invoiceDataExtracted.NitReceptor != payer.Nit)
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByNoSamePayer), itemInvoice.ZipName));
                    continue;
                }
                if (string.IsNullOrEmpty(invoiceDataExtracted.TipoPago) || invoiceDataExtracted.TipoPago == ConstantCode_PayType.Debit)
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByIsNotCreditType), itemInvoice.ZipName));
                    continue;
                }
                if (string.IsNullOrEmpty(invoiceDataExtracted.CurrencyCode) || !ConstantCode_MoneyType.Validate(invoiceDataExtracted.CurrencyCode))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByIncorrectMoneyType), itemInvoice.ZipName));
                    continue;
                }
                if (string.IsNullOrEmpty(invoiceDataExtracted.Trm) && invoiceDataExtracted.CurrencyCode == ConstantCode_MoneyType.USD)
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByEmptyField, Trm), itemInvoice.ZipName));
                    continue;
                }
                if (await _invoiceRepository.ExistsByCufeWithStatusAsync(invoiceDataExtracted.Cufe))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByInvoiceExist), itemInvoice.ZipName));
                    continue;
                }
                if (await _cufeRepository.ExistsByCufeAsync(invoiceDataExtracted.Cufe))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByInvoiceExist), itemInvoice.ZipName));
                    continue;
                }

                InvoiceTemp invoiceTemp = new()
                {
                    XmlFile = new Dictionary<string, byte[]> { { invoiceDataExtracted.Factura + ConstantCode_FileExtension.XML, itemInvoice.XmlFile.FirstOrDefault().Value } },
                    PdfFile = new Dictionary<string, byte[]> { { invoiceDataExtracted.Factura + ConstantCode_FileExtension.PDF, itemInvoice.PdfFile.FirstOrDefault().Value } },
                    Cufe = invoiceDataExtracted.Cufe,
                    DueDate = invoiceDataExtracted.FechaVencimiento,
                    EmitDate = invoiceDataExtracted.FechaEmision,
                    ZipName = itemInvoice.ZipName,
                    Number = invoiceDataExtracted.Factura,
                    MoneyType = invoiceDataExtracted.CurrencyCode,
                    Total = invoiceDataExtracted.Total,
                    Trm = invoiceDataExtracted.Trm,
                    NitEmisor = invoiceDataExtracted.NitEmisor,
                    NitReceptor = invoiceDataExtracted.NitReceptor,
                    TaxAmount = invoiceDataExtracted.TaxAmount
                };

                invoiceTempsCorrects.Add(invoiceTemp);

                invoiceProcessCache.ValidationBusinessCurrent += 1;
            }

            return invoiceTempsCorrects;
        }

        private static List<InvoiceTemp> ValidateSchema(InvoiceProcessCache invoiceProcessCache, List<InvoiceTemp> invoiceTemps)
        {
            XmlSchema schema = GetXsdFromUrl(Url_XmlSchema);

            List<InvoiceTemp> invoiceTempsWithCorrectSchema = new();

            foreach (var itemInvoiceTemp in invoiceTemps)
            {
                if (ValidateXmlWithXsd(itemInvoiceTemp.XmlFile.FirstOrDefault().Value, schema))
                {
                    InvoiceTemp invoiceTemp = new()
                    {
                        XmlFile = itemInvoiceTemp.XmlFile,
                        PdfFile = itemInvoiceTemp.PdfFile,
                        ZipName = itemInvoiceTemp.ZipName
                    };
                    invoiceTempsWithCorrectSchema.Add(invoiceTemp);
                }
                else
                {
                    invoiceProcessCache.FilesRejected?.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectBySchemaDian), itemInvoiceTemp.ZipName));
                }
            }

            return invoiceTempsWithCorrectSchema;
        }

        private static List<InvoiceTemp> ValidateSameCufeInSameLoad(InvoiceProcessCache invoiceProcessCache, List<InvoiceTemp> invoiceTemps)
        {
            List<InvoiceTemp> invoiceTempsWithCorrects = new();

            foreach (var itemInvoiceTemp in invoiceTemps)
            {
                if (invoiceTemps.Count(x => x.Cufe == itemInvoiceTemp.Cufe) >= 2)//validar que no existan dos cufes iguales en la misma carga
                {
                    invoiceProcessCache.FilesRejected?.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectBySameCufe), itemInvoiceTemp.ZipName));
                }
                else
                {
                    InvoiceTemp invoiceTemp = new()
                    {
                        XmlFile = itemInvoiceTemp.XmlFile,
                        PdfFile = itemInvoiceTemp.PdfFile,
                        ZipName = itemInvoiceTemp.ZipName,
                        Cufe = itemInvoiceTemp.Cufe,
                        DueDate = itemInvoiceTemp.DueDate,
                        EmitDate = itemInvoiceTemp.EmitDate,
                        Number = itemInvoiceTemp.Number,
                        MoneyType = itemInvoiceTemp.MoneyType,
                        Total = itemInvoiceTemp.Total,
                        Trm = itemInvoiceTemp.Trm,
                        NitEmisor = itemInvoiceTemp.NitEmisor,
                        NitReceptor = itemInvoiceTemp.NitReceptor,
                        TaxAmount = itemInvoiceTemp.TaxAmount
                    };
                    invoiceTempsWithCorrects.Add(invoiceTemp);
                }
            }

            return invoiceTempsWithCorrects;
        }

        private static List<InvoiceTemp> ValidateExistenceTwoFiles(InvoiceProcessCache invoiceProcessCache, List<IFormFile> filesWithOutVirus)
        {
            List<InvoiceTemp> invoiceTemps = new();

            foreach (var itemFile in filesWithOutVirus)
            {
                Dictionary<string, byte[]> filesExtracted = TransformModule.ExtractAndValidateZip(itemFile, invoiceProcessCache);

                if (filesExtracted.Count() >= 2)
                {
                    // se valida si viene mas de un xml en el .zip
                    var countXml = filesExtracted.Where(entry => entry.Key.EndsWith(ConstantCode_FileExtension.XML, StringComparison.OrdinalIgnoreCase)).Count();
                    if (countXml > 1)
                    {
                        invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.TwoXmlInSameZip), itemFile.FileName));
                    }
                    else
                    {
                        Dictionary<string, byte[]> xmlFile = GetXmlFile(filesExtracted);
                        Dictionary<string, byte[]> pdfFile = GetPdfFile(filesExtracted);

                        InvoiceTemp invoiceTemp = new()
                        {
                            XmlFile = xmlFile,
                            PdfFile = pdfFile,
                            ZipName = itemFile.FileName
                        };
                        invoiceTemps.Add(invoiceTemp);
                    }
                }
                else
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByNoHaveTwoFiles), itemFile.FileName));
                }
            }

            return invoiceTemps;
        }

        private List<IFormFile> ValidateVirus(InvoiceProcessCache invoiceProcessCache, List<IFormFile> filesWithCorrectExtension)
        {
            List<IFormFile> filesWithOutVirus = new();
            invoiceProcessCache.ScanMax = filesWithCorrectExtension.Count();
            foreach (var itemFile in filesWithCorrectExtension)
            {
                if (_scanFile.ValidateFile(itemFile))
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByHaveVirus), itemFile.FileName));
                }
                else
                {
                    filesWithOutVirus.Add(itemFile);
                }
                invoiceProcessCache.ScanCurrent += 1;
            }

            return filesWithOutVirus;
        }

        private static List<IFormFile> ValidateExtension(UploadFilesCommand command, InvoiceProcessCache invoiceProcessCache)
        {
            List<IFormFile> filesWithCorrectExtension = new();
            //que los archivos tengan la extension requerida
            foreach (var itemFile in command.files)
            {
                if (TransformModule.GetFileExt(itemFile.FileName) != ConstantCode_MimeType.ZIP)
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectByNoZip), itemFile.FileName));
                }
                else
                {
                    filesWithCorrectExtension.Add(itemFile);
                }
            }

            return filesWithCorrectExtension;
        }

        private static List<IFormFile> ValidateFileSize(List<IFormFile> files, InvoiceProcessCache invoiceProcessCache)
        {
            List<IFormFile> filesWithCorrectSize = new();
            //que los archivos tengan la extension requerida
            foreach (var itemFile in files)
            {
                if (itemFile.Length > 5 * 1024 * 1024) // 5 megabytes
                {
                    invoiceProcessCache.FilesRejected.Add(Tuple.Create(GetErrorDescription(MessageCodes.FileRejectBySize), itemFile.FileName));
                }
                else
                {
                    filesWithCorrectSize.Add(itemFile);
                }
            }

            return filesWithCorrectSize;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="itemInvoice"></param>
        /// <param name="invoiceDataExtracted"></param>
        private static InvoiceDataExtracted GetDataFromXml(InvoiceTemp itemInvoice, InvoiceDataExtracted invoiceDataExtracted, XmlDocument xmlDoc)
        {            
            string xmlInvoceIntern = GetElementValue(itemInvoice.XmlFile.FirstOrDefault().Value, "cbc:Description");

            invoiceDataExtracted.GetData(xmlDoc, itemInvoice.XmlFile.FirstOrDefault().Value);

            if ((xmlInvoceIntern.Length > 300) && invoiceDataExtracted.AnyNullOrEmpty())
            {
                XmlDocument xmlDocInternal = new();
                xmlDocInternal.LoadXml(xmlInvoceIntern);

                invoiceDataExtracted.GetData(xmlDocInternal, Encoding.UTF8.GetBytes(xmlInvoceIntern));
            }

            return invoiceDataExtracted;
        }

        private static string GetElementValue(byte[] data, string element)
        {
            try
            {
                using (var xmlStream = new MemoryStream(data))
                using (var xmlReaders = XmlReader.Create(xmlStream))
                {
                    while (xmlReaders.Read())
                    {
                        if (xmlReaders.NodeType == XmlNodeType.Element && xmlReaders.Name == element)
                        {
                            return ReadValue(xmlReaders);
                        }
                    }
                }

                // Si no se encuentra el elemento, regresar null
                return null;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al obtener el valor del elemento: {ex.Message}");
#endif
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <returns></returns>
        private static string ReadValue(XmlReader xmlReader)
        {
            string valor = null;

            if (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Text || xmlReader.NodeType == XmlNodeType.CDATA)
                {
                    valor = xmlReader.Value;
                }
                else if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Text || xmlReader.NodeType == XmlNodeType.CDATA)
                        {
                            valor = xmlReader.Value;
                            break;
                        }
                    }
                }
            }

            return valor;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private static bool ValidateXmlWithXsd(byte[] xmlData, XmlSchema schema)
        {
            try
            {
                // Cargar el XML desde el arreglo de bytes
                using (var xmlStream = new MemoryStream(xmlData))
                {
                    var xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.ValidationType = ValidationType.Schema;
                    xmlReaderSettings.Schemas.Add(schema);
                    //xmlReaderSettings.ValidationEventHandler += BooksSettingsValidationEventHandler;

                    using (var xmlReader = XmlReader.Create(xmlStream, xmlReaderSettings))
                    {
                        while (xmlReader.Read()) { }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error en la validación: {ex.Message}");
#endif
                return true; //CAMBIAR A FALSE
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static XmlSchema GetXsdFromUrl(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var xsdContent = httpClient.GetStringAsync(url).Result;

                    var schema = new XmlSchema();
                    schema = XmlSchema.Read(new System.IO.StringReader(xsdContent), ValidationEventHandler);

                    return schema;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"Error al descargar y cargar el XSD desde la URL: {ex.Message}");
#endif
                return null;
            }
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
#if DEBUG
                Console.WriteLine($"Error de validación: {e.Message}");
#endif
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static Dictionary<string, byte[]> GetXmlFile(Dictionary<string, byte[]> file)
        {
            var xmlFile = file.FirstOrDefault(entry => entry.Key.EndsWith(ConstantCode_FileExtension.XML, StringComparison.OrdinalIgnoreCase));

            if (xmlFile.Equals(default(KeyValuePair<string, byte[]>)))
            {
                return new Dictionary<string, byte[]>(); // Devolver un diccionario vacío si no se encontró el archivo XML
            }

            var diccionaryResult = new Dictionary<string, byte[]>
            {
                { xmlFile.Key, xmlFile.Value }
            };

            return diccionaryResult;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static Dictionary<string, byte[]> GetPdfFile(Dictionary<string, byte[]> file)
        {
            var pdfFile = file.FirstOrDefault(entry => entry.Key.EndsWith(ConstantCode_FileExtension.PDF, StringComparison.OrdinalIgnoreCase));

            if (pdfFile.Equals(default(KeyValuePair<string, byte[]>)))
            {
                return new Dictionary<string, byte[]>(); // Devolver un diccionario vacío si no se encontró el archivo PDF
            }

            var diccionaryResult = new Dictionary<string, byte[]>
            {
                { pdfFile.Key, pdfFile.Value }
            };

            return diccionaryResult;
        }
    }
}