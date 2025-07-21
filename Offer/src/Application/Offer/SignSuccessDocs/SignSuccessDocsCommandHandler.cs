///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ClosedXML.Excel;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.FtpFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Offer.Application.HistoricalStates.Add;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.HistoricalStates;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using System.Data;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.SignSuccessDocs
{
    public sealed class SignSuccessDocsCommandHandler : IRequestHandler<SignSuccessDocsCommand, ErrorOr<bool>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IZapsign _Zapsign;
        private readonly IDocumentRepository _documentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IFtp _ftp;
        private readonly IHistoricalStatesRepository historicalStatesRepository;
        private readonly IMediator mediator;
        private readonly IUnitOfWork _unitOfWork;

        private const string columOne = "TIPO DE DOCUMENTO";
        private const string columTwo = "ID OFERTA";
        private const string columThree = "FACTURA NO.";
        private const string columFour = "NIT  EMISOR";
        private const string columFive = "NOMBRE DEL EMISOR";
        private const string columSix = "NIT PAGADOR";
        private const string columSeven = "NOMBRE DEL PAGADOR";
        private const string columEight = "FECHA DE EMISIÓN";
        private const string columNine = "FECHA DE VENCIMIENTO";
        private const string columTen = "FECHA DE PAGO CONFIRMADA";
        private const string columEleven = "VALOR NETO DE PAGO";

        //private const string AnexoSigned = "AnexoSigned.pdf";
        private const string OfertaMercantilSigned = "Oferta Mercantil y Anexo Firmado.pdf";

        private const string InstruccionGiroSigned = "Instruccion de Giro Firmado.pdf";
        private const string EndosoDeDocumentosSigned = "Documento de Endoso Firmado.pdf";
        private const string NotificacionEndosoSigned = "Notificacion de Endoso Firmado.pdf";
        private const string InstruccionGiroExcel = "Instruccion de Giro.xlsx";

        //private const string AnexoName = "Anexo.pdf";
        private const string InstruccionGiroName = "Instruccion de Giro.pdf";

        private const string EndosoDeDocumentosName = "Documento de Endoso.pdf";
        private const string NotificacionEndosoName = "Notificacion de Endoso.pdf";
        private const string Storage = "storage";
        private const string DocumentsSeller = "Documents/Seller";
        private const string Subject = "Oferta habilitada ";

        public SignSuccessDocsCommandHandler(IUnitOfWork unitOfWork, IStorage storage, IOfferRepository offerRepository,
            IZapsign zapsign, IUserRepository userRepository, IDocumentRepository documentRepository, ICatalogBusiness catalogBusiness,
            IInvoiceRepository invoiceRepository, IFtp ftp, IHistoricalStatesRepository historicalStatesRepository, IMediator mediator)
        {
            _Zapsign = zapsign ?? throw new ArgumentNullException(nameof(zapsign));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _ftp = ftp ?? throw new ArgumentNullException(nameof(ftp));
            this.historicalStatesRepository = historicalStatesRepository;
            this.mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(SignSuccessDocsCommand command, CancellationToken cancellationToken)
        {
            //string anexoUrlSignedFile = null;
            string instruccionGiroUrlSignedFile = null;
            string endosoUrlSignedFile = null;
            string notificacionEndosoUrlSignedFile = null;

            Domain.Offer offer = await _offerRepository.GetByIdAsync(command.offerId);
            if (offer == null || offer.StatusId != CatalogCode_OfferStatus.InProgress)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var documents = await _documentRepository.GetAllDocumentsByOfferAsync(command.offerId);

            //var docAppendix = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Appendix);
            var docMoneyTransferInstruction = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstruction);
            var docCommercialOffer = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOffer);
            var docEndorsement = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.Endorsement);
            var docEndorsementNotification = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.EndorsementNotification);
            var docMoneyTransferInstructionExcel = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstructionExcel);

            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

            var user = await _userRepository.GetByIdAsync(offer.UserId);

            if (string.IsNullOrEmpty(docCommercialOffer.TokenZapsign))
            {
                return Error.Validation(MessageCodes.ZapsignNoToken, GetErrorDescription(MessageCodes.ZapsignNoToken));
            }

            //Obtener los documentos firmados desde zapsign
            var zapsignResponse = await _Zapsign.GetDetailAsync(docCommercialOffer.TokenZapsign);

            if (zapsignResponse.status == "pending")
            {
                return Error.Validation(MessageCodes.ZapsignError, GetErrorDescription(MessageCodes.ZapsignError));
            }

            //si hay algun anexo que no traiga el documento firmado se intenta traer con el token de cada uno. De lo contrario se extraen del documento principal
            if (zapsignResponse.extra_docs.Any(x => x.signed_file == null))
            {
                //Intentar obtener los documentos anexados firmados
                //var anexo = await _Zapsign.GetDetailAsync(zapsignResponse.extra_docs.Where(x => x.name == AnexoName).FirstOrDefault().token);
                var instruccionGiro = await _Zapsign.GetDetailAsync(zapsignResponse.extra_docs.Where(x => x.name == InstruccionGiroName).FirstOrDefault().token);
                var endoso = await _Zapsign.GetDetailAsync(zapsignResponse.extra_docs.Where(x => x.name == EndosoDeDocumentosName).FirstOrDefault().token);
                var notificacionEndoso = await _Zapsign.GetDetailAsync(zapsignResponse.extra_docs.Where(x => x.name == NotificacionEndosoName).FirstOrDefault().token);

                if (instruccionGiro == null || endoso == null || notificacionEndoso == null ||
                    instruccionGiro.signed_file == null || endoso.signed_file == null || notificacionEndoso.signed_file == null)
                {
                    return Error.Validation(MessageCodes.ZapsignError, GetErrorDescription(MessageCodes.ZapsignError));
                }
                else
                {
                    //anexoUrlSignedFile = anexo.signed_file;
                    instruccionGiroUrlSignedFile = instruccionGiro.signed_file;
                    endosoUrlSignedFile = endoso.signed_file;
                    notificacionEndosoUrlSignedFile = notificacionEndoso.signed_file;
                }
            }
            else
            {
                //se extraen del documento principal
                //anexoUrlSignedFile = zapsignResponse.extra_docs.Where(x => x.name == AnexoName).FirstOrDefault().signed_file;
                instruccionGiroUrlSignedFile = zapsignResponse.extra_docs.Where(x => x.name == InstruccionGiroName).FirstOrDefault().signed_file;
                endosoUrlSignedFile = zapsignResponse.extra_docs.Where(x => x.name == EndosoDeDocumentosName).FirstOrDefault().signed_file;
                notificacionEndosoUrlSignedFile = zapsignResponse.extra_docs.Where(x => x.name == NotificacionEndosoName).FirstOrDefault().signed_file;
            }

            //almacenar en el storage los documentos firmados
            string storageRute = $"{Storage}/{offer.Consecutive}/{DocumentsSeller}/";

            //object urlAnexo = await _storage.UploadAsync(await DownloadFileAsync(anexoUrlSignedFile), storageRute + AnexoSigned);
            object urlOfertaMercantil = await _storage.UploadAsync(await DownloadFileAsync(zapsignResponse.signed_file), storageRute + OfertaMercantilSigned);
            object urlInstruccionGiro = await _storage.UploadAsync(await DownloadFileAsync(instruccionGiroUrlSignedFile), storageRute + InstruccionGiroSigned);
            object urlEndoso = await _storage.UploadAsync(await DownloadFileAsync(endosoUrlSignedFile), storageRute + EndosoDeDocumentosSigned);
            object urlNotificacionEndoso = await _storage.UploadAsync(await DownloadFileAsync(notificacionEndosoUrlSignedFile), storageRute + NotificacionEndosoSigned);

            //generar excel, almacenarlo en el storage y guardar el registro en la tabla document
            string nameExcel = $"OPER_{offer.Consecutive}_{string.Format("{0:ddMMyyyy}", ExtensionFormat.DateTimeCO())}.xlsx";
            var fileExcel = await CreateFileExcelAsync(command.offerId);
            object urlFileExcel = await _storage.UploadAsync(fileExcel, storageRute + nameExcel);
            Document documentExcel = new(new Guid(), command.offerId, null, nameExcel, CatalogCode_DocumentType.FactoringIn, false, urlFileExcel.ToString(),
                fileExcel.LongLength.ToMegaByte());

            //se actualizan los documentos
            await SaveInDB(docMoneyTransferInstruction, docCommercialOffer, docEndorsement, docEndorsementNotification, urlOfertaMercantil, urlInstruccionGiro,
            urlEndoso, urlNotificacionEndoso, documentExcel, offer, cancellationToken);

            await SendEmails(offer, user, urlOfertaMercantil, urlInstruccionGiro, nameExcel, urlFileExcel, urlEndoso, urlNotificacionEndoso, docMoneyTransferInstructionExcel.Url);

            await _ftp.SendFileToFactoringAsync(fileExcel, nameExcel);

            return true;
        }

        private async Task SendEmails(Domain.Offer offer, Domain.Users.User user, object urlOfertaMercantil,
            object urlInstruccionGiro, string nameExcel, object urlFileExcel, object urlEndoso, object urlNotificacionEndoso, object urlMoneyTransferInstructionExcel)
        {
            var time = await _catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.TimeEnableLink);
            var enlaceyourInvoice = await _catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.UrlyourInvoice);
            //string enlaceAnexo = await _storage.GenerateSecureDownloadUrlAsync(urlAnexo.ToString(), AnexoSigned, Convert.ToInt32(time.Descripton));
            string enlaceOfertaComercial = await _storage.GenerateSecureDownloadUrlAsync(urlOfertaMercantil.ToString(), OfertaMercantilSigned, Convert.ToInt32(time.Descripton));
            string enlaceInstruccionGiro = await _storage.GenerateSecureDownloadUrlAsync(urlInstruccionGiro.ToString(), InstruccionGiroSigned, Convert.ToInt32(time.Descripton));
            string enlaceFileExcel = await _storage.GenerateSecureDownloadUrlAsync(urlFileExcel.ToString(), nameExcel, Convert.ToInt32(time.Descripton));
            string enlaceEndoso = await _storage.GenerateSecureDownloadUrlAsync(urlEndoso.ToString(), EndosoDeDocumentosSigned, Convert.ToInt32(time.Descripton));
            string enlaceNotificacionEndoso = await _storage.GenerateSecureDownloadUrlAsync(urlNotificacionEndoso.ToString(), NotificacionEndosoSigned, Convert.ToInt32(time.Descripton));
            string enlaceInstruccionGiroExcel = await _storage.GenerateSecureDownloadUrlAsync(urlMoneyTransferInstructionExcel.ToString(), InstruccionGiroExcel, Convert.ToInt32(time.Descripton));

            var templateAdmin = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToAdmin);
            var templateSeller = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToSeller);
            var information = await _invoiceRepository.GetInvoiceSumPayerSellerAsync(offer.Id);
            Dictionary<string, string> replacements = new()
            {
                { "{{vendedor}}", information.SellerCompany },
                { "{{pagador}}", information.PayerName },
                { "{{valor_total}}", string.Format("{0:C}", information.InvoiceSum)},
                { "{{urlyourInvoice}}", enlaceyourInvoice.Descripton },
                { "{{urlOfertaComercial}}", enlaceOfertaComercial },
                { "{{urlInstruccionGiro}}", enlaceInstruccionGiro },
                { "{{urlFileExcel}}", enlaceFileExcel },
                { "{{oferta_mercantil}}", offer.Consecutive.ToString() },
                { "{{urlEndoso}}", enlaceEndoso },
                { "{{urlNotificacionEndoso}}", enlaceNotificacionEndoso },
                { "{{urlInstruccionGiroExcel}}", enlaceInstruccionGiroExcel },
                { "{{year}}", ExtensionFormat.DateTimeCO().Year.ToString() }
            };

            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, replacements);
            string templateSellerWithData = TransformModule.ReplaceTokens(templateSeller.Descripton, replacements);

            EmainBusiness emainBusiness = new(_catalogBusiness);
            //email para admin
            var emailAdmin = await _userRepository.GetEmailRoleAsync(CatalogCode_UserRole.Administrator);
            await emainBusiness.SendAsync(emailAdmin, Subject + offer.Consecutive, templateAdminWithData);
            //email para seller
            await emainBusiness.SendAsync(user.Email, Subject + offer.Consecutive, templateSellerWithData);
        }

        private async Task<byte[]> CreateFileExcelAsync(Guid offerId)
        {
            var documentType = await _catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.DocumentType);
            var invoices = _invoiceRepository.ListToGenerateExcel(offerId);
            DataTable dt = new DataTable("DataInvoice");
            dt.Columns.AddRange(new DataColumn[] { new DataColumn(columOne), new DataColumn(columTwo), new DataColumn(columThree), new DataColumn(columFour)
            , new DataColumn(columFive), new DataColumn(columSix), new DataColumn(columSeven), new DataColumn(columEight), new DataColumn(columNine)
            , new DataColumn(columTen), new DataColumn(columEleven)});
            invoices.ToList().ForEach(inv => dt.Rows.Add(documentType.Descripton, inv.OfferConsecutive, inv.InvoiceNumber, inv.SellerNit, inv.SellerName,
                inv.PayerNit, inv.PayerName, inv.EmitDate, inv.DueDate, inv.NegotiationDate, inv.NegotiationTotal));
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var sheet = workbook.AddWorksheet(dt, documentType.Descripton);
                sheet.Columns(1, 12).Width = 25;
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        private async Task SaveInDB(Document docMoneyTransferInstruction, Document docCommercialOffer, Document docEndoso, Document docNotificacionEndoso,
            object urlOfertaMercantil, object urlInstruccionGiro, object urlEndoso, object urlNotificacionEndoso, Document documentExcel, Domain.Offer offer, CancellationToken cancellationToken)
        {
            docCommercialOffer.IsSigned = true;
            docCommercialOffer.Url = urlOfertaMercantil.ToString();
            docCommercialOffer.Name = OfertaMercantilSigned;
            docMoneyTransferInstruction.IsSigned = true;
            docMoneyTransferInstruction.Url = urlInstruccionGiro.ToString();
            docMoneyTransferInstruction.Name = InstruccionGiroSigned;
            //docAppendix.IsSigned = true;
            //docAppendix.Url = urlAnexo.ToString();
            //docAppendix.Name = AnexoSigned;
            docEndoso.IsSigned = true;
            docEndoso.Url = urlEndoso.ToString();
            docEndoso.Name = EndosoDeDocumentosSigned;
            docNotificacionEndoso.IsSigned = true;
            docNotificacionEndoso.Url = urlNotificacionEndoso.ToString();
            docNotificacionEndoso.Name = NotificacionEndosoSigned;
            //Se actualizan documentos
            _documentRepository.Update(docCommercialOffer);
            _documentRepository.Update(docMoneyTransferInstruction);
            //_documentRepository.Update(docAppendix);
            _documentRepository.Update(docEndoso);
            _documentRepository.Update(docNotificacionEndoso);
            //Se adiciona nuevo documento de excel
            _documentRepository.Add(documentExcel);
            //se actuliza el estado de la oferta
            offer.StatusId = CatalogCode_OfferStatus.Enabled;
            _offerRepository.Update(offer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await this.mediator.Publish(new AddHistoricalCommand { OfferId = offer.Id, StatusId = CatalogCode_OfferStatus.Enabled, UserId = offer.UserId }, cancellationToken);
        }

        private static async Task<byte[]> DownloadFileAsync(string urlDescarga)
        {
            using (HttpClient cliente = new())
            {
                try
                {
                    // Descargar el contenido de la URL como array de bytes
                    byte[] archivoBytes = await cliente.GetByteArrayAsync(urlDescarga);
                    return archivoBytes;
                }
                catch (HttpRequestException ex)
                {
                    // Manejar cualquier excepción de descarga aquí
#if DEBUG
                    Console.WriteLine($"Error al descargar el archivo: {ex.Message}");
#endif
                    return null;
                }
            }
        }
    }
}