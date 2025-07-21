///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using ClosedXML.Excel;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.PdfModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using System.Data;
using System.Globalization;
using System.Text;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Generate
{
    public sealed class GenerateDocsCommandHandler : IRequestHandler<GenerateDocsCommand, ErrorOr<bool>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMoneyTransferRepository _moneyTransferRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorage _storages;
        private readonly ICatalogBusiness _catalogBusinesss;
        private readonly IPayerRepository _payerRepository;
        private readonly IUnitOfWork _unitOfWork;

        private const string beneficiaryName = "Nombre del beneficiario";
        private const string identificationType = "Tipo de identificacion";
        private const string identificationNumber = "Numero de identificacion";
        private const string destinationFinancialInstitution = "Entidad financiera destino";
        private const string destinationAccountNumber = "Numero de cuenta destino";
        private const string destinationAccountType = "Tipo cuenta destino";

        //private const string AnexoName = "Anexo.pdf";
        private const string InstruccionGiroName = "Instruccion de Giro.pdf";

        private const string EndosoDeDocumentosName = "Documento de Endoso.pdf";
        private const string NotificacionEndosoName = "Notificacion de Endoso.pdf";
        private const string InstruccionGiroExcelName = "Instruccion de Giro.xlsx";
        private const string OfertaMercantilName = "Oferta Mercantil y Anexo.pdf";
        private const string Storage = "storage";
        private const string DocumentsSeller = "Documents/Seller";

        private CultureInfo colombianCulture = new CultureInfo("es-CO");

        public GenerateDocsCommandHandler(IUnitOfWork unitOfWork, IStorage storage, IOfferRepository offerRepository,
            IMoneyTransferRepository moneyTransferRepository, IUserRepository userRepository, IDocumentRepository documentRepository,
            IInvoiceRepository invoiceRepository, ICatalogBusiness catalogBusiness, IPayerRepository payerRepository)
        {
            this._moneyTransferRepository = moneyTransferRepository ?? throw new ArgumentNullException(nameof(moneyTransferRepository));
            this._offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storages = storage ?? throw new ArgumentNullException(nameof(storage));
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _catalogBusinesss = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            _payerRepository = payerRepository ?? throw new ArgumentNullException(nameof(payerRepository));
        }

        public async Task<ErrorOr<bool>> Handle(GenerateDocsCommand command, CancellationToken cancellationToken)
        {
            //si el estado de la oferta no es en progreso saca error
            if (!await _offerRepository.OfferIsInProgressAsync(command.offerId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            Domain.Offer offer = await _offerRepository.GetByIdAsync(command.offerId);
            if (offer == null || offer.StatusId != CatalogCode_OfferStatus.InProgress)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            //validar si los documentos ya habian sido generados previamente y si ya estan firmados.
            var documents = await _documentRepository.GetAllDocumentsByOfferAsync(command.offerId);
            var anyIsSigned = documents.Any(x => x.TypeId == CatalogCode_DocumentType.CommercialOffer && x.IsSigned == true);
            if (anyIsSigned)
                return Error.Validation(MessageCodes.DocumentIsSigned, GetErrorDescription(MessageCodes.DocumentIsSigned));

            var payer = await _payerRepository.GetByIdAsync(offer.PayerId);
            var user = await _userRepository.GetByIdAsync(offer.UserId);
            Guid? personType = await _userRepository.GetPersonTypeByIdAsync(offer.UserId);
            string date = ExtensionFormat.DateTimeCO().ToString("d 'de' MMMM yyyy", new CultureInfo("es-ES", false));
            string city = user.City;

            bool isNatural = false;
            if (personType == CatalogCode_PersonType.Natural)
                isNatural = true;

#warning AQUI HACE FALTA LA CONSULTA Y VALIDACION SI ES CON O SIN RESPONSABILIDAD, DEBE SER IMPLEMENTADO DESPUES DE VINCULACION
            string isWithResponsability = null;
            if (true)
                isWithResponsability = "sin";
            else
                isWithResponsability = "con";

            MemoryStream pdfCommercialOffer = await GenerateCommercialOfferDocument(command, offer, user, date, isNatural, isWithResponsability);
            MemoryStream pdfEndorsement = await GenerateEndorsementDocument(command, city, date, user, payer, isNatural, isWithResponsability);
            MemoryStream pdfEndorsementNotification = await GenerateEndorsementNotificationDocument(command, city, date, user, payer, isNatural);

            //MemoryStream pdfAppendix = await GenerateAppendix1Document(command, user);
            MemoryStream pdfMoneyTransferInstruction = await GenerateMoneyTransferInstructionDocument(command, user, date, isNatural, city, offer.Consecutive.ToString());
            MemoryStream excelMoneyTransferInstruction = await GenerateMoneyTransferInstructionDocumentExcel(command);

            string storageRute = $"{Storage}/{offer.Consecutive}/{DocumentsSeller}/";

            object urlEndorsement = await _storages.UploadAsync(pdfEndorsement.ToArray(), storageRute + EndosoDeDocumentosName);
            object urlEndorsementNotification = await _storages.UploadAsync(pdfEndorsementNotification.ToArray(), storageRute + NotificacionEndosoName);
            //object urlAnexo = await _storages.UploadAsync(pdfAppendix.ToArray(), storageRute + AnexoName);
            object urlOfertaMercantil = await _storages.UploadAsync(pdfCommercialOffer.ToArray(), storageRute + OfertaMercantilName);
            object urlInstruccionGiro = await _storages.UploadAsync(pdfMoneyTransferInstruction.ToArray(), storageRute + InstruccionGiroName);
            object urlInstruccionGirowExcel = await _storages.UploadAsync(excelMoneyTransferInstruction.ToArray(), storageRute + InstruccionGiroExcelName);

            await SaveInDB(command, urlOfertaMercantil, urlInstruccionGiro, urlEndorsement, urlEndorsementNotification, urlInstruccionGirowExcel, cancellationToken, 
                pdfCommercialOffer.ToArray().LongLength, pdfMoneyTransferInstruction.ToArray().LongLength, pdfEndorsement.ToArray().LongLength, pdfEndorsementNotification.ToArray().LongLength, 
                excelMoneyTransferInstruction.Length, documents);

            return true;
        }

        private async Task SaveInDB(GenerateDocsCommand command, object urlOfertaMercantil,
            object urlInstruccionGiro, object urlEndorsement, object urlEndorsementNotification, object urlInstruccionGirowExcel, CancellationToken cancellationToken,
            long sizeCommercialOffer, long sizeMti, long sizeEndorsement, long sizeEndorsementNotification, long sizeMtiExcel,
            List<Document> documents)
        {
            //si ya existe el documento en la tabla se consulta y se actualiza
            //var appendix = documents.Where(x => x.TypeId == CatalogCode_DocumentType.Appendix).FirstOrDefault();
            var moneyTransferInstruction = documents.Where(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstruction).FirstOrDefault();
            var commercialOffer = documents.Where(x => x.TypeId == CatalogCode_DocumentType.CommercialOffer).FirstOrDefault();
            var endorsement = documents.Where(x => x.TypeId == CatalogCode_DocumentType.Endorsement).FirstOrDefault();
            var endorsementNotification = documents.Where(x => x.TypeId == CatalogCode_DocumentType.EndorsementNotification).FirstOrDefault();
            var moneyTransferInstructionExcel = documents.Where(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstructionExcel).FirstOrDefault();

            //Document Anexo = new(Guid.NewGuid(), command.offerId, null, AnexoName,
            //    CatalogCode_DocumentType.Appendix, false, urlAnexo.ToString(), sizeAppendix.ToMegaByte());
            Document OfertaMercantil = new(Guid.NewGuid(), command.offerId, null, OfertaMercantilName, CatalogCode_DocumentType.CommercialOffer, false, urlOfertaMercantil.ToString(), (sizeCommercialOffer * 10).ToMegaByte());
            Document InstruccionGiro = new(Guid.NewGuid(), command.offerId, null, InstruccionGiroName, CatalogCode_DocumentType.MoneyTransferInstruction, false, urlInstruccionGiro.ToString(), (sizeMti * 10).ToMegaByte());
            Document Endoso = new(Guid.NewGuid(), command.offerId, null, EndosoDeDocumentosName, CatalogCode_DocumentType.Endorsement, false, urlEndorsement.ToString(), (sizeEndorsement * 10).ToMegaByte());
            Document NotificacionEndoso = new(Guid.NewGuid(), command.offerId, null, NotificacionEndosoName, CatalogCode_DocumentType.EndorsementNotification, false, urlEndorsementNotification.ToString(), (sizeEndorsementNotification * 10).ToMegaByte());
            Document InstruccionGiroExcel = new(Guid.NewGuid(), command.offerId, null, InstruccionGiroExcelName, CatalogCode_DocumentType.MoneyTransferInstructionExcel, false, urlInstruccionGirowExcel.ToString(), sizeMtiExcel.ToMegaByte());

            if (commercialOffer == null)
            {
                //_documentRepository.Add(Anexo);
                _documentRepository.Add(OfertaMercantil);
                _documentRepository.Add(InstruccionGiro);
                _documentRepository.Add(Endoso);
                _documentRepository.Add(NotificacionEndoso);
                _documentRepository.Add(InstruccionGiroExcel);
            }
            else
            {
                //appendix.FileSize = sizeAppendix.ToMegaByte();
                moneyTransferInstruction.FileSize = (sizeMti * 10).ToMegaByte();
                commercialOffer.FileSize = (sizeCommercialOffer * 10).ToMegaByte();
                endorsement.FileSize = (sizeEndorsement * 10).ToMegaByte();
                endorsementNotification.FileSize = (sizeEndorsementNotification * 10).ToMegaByte();
                moneyTransferInstructionExcel.FileSize = sizeMtiExcel.ToMegaByte();
                //_documentRepository.Update(appendix);
                _documentRepository.Update(moneyTransferInstruction);
                _documentRepository.Update(commercialOffer);
                _documentRepository.Update(endorsement);
                _documentRepository.Update(endorsementNotification);
                _documentRepository.Update(moneyTransferInstructionExcel);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<MemoryStream> GenerateMoneyTransferInstructionDocument(GenerateDocsCommand command, Domain.Users.User seller, string date, bool isNatural, string city, string consecutive)
        {
            yourInvoice.Common.Entities.CatalogItemInfo template = null;

            if (isNatural)
                template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PdfMoneyTransferInstructionPn);
            else
                template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PdfMoneyTransferInstructionPj);

            var nityourInvoice = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.Nit);
            var documentType = await _catalogBusinesss.GetByIdAsync(seller.DocumentTypeId);
            var responseMoneyTrans = await _moneyTransferRepository.ListToMoneyTransferDocumentAsync(command.offerId);

            StringBuilder tableContent = new();

            foreach (var itemResponseMoneyTrans in responseMoneyTrans.TableContent)
            {
                // Agregar cadenas al StringBuilder
                tableContent.Append("<tr>");
                tableContent.Append("<td>" + itemResponseMoneyTrans.Name + "</td>");
                tableContent.Append("<td>" + itemResponseMoneyTrans.DocumentNumber + "</td>");
                tableContent.Append("<td>" + itemResponseMoneyTrans.Bank + "</td>");
                tableContent.Append("<td>" + itemResponseMoneyTrans.AccountNumber + "</td>");
                tableContent.Append("<td>" + itemResponseMoneyTrans.Total + "</td>");
                tableContent.Append("</tr>");
            }

            Dictionary<string, string> replacements = new()
            {
                { "{{ciudad}}", city },
                { "{{fecha}}", date },
                { "{{consecutive}}", consecutive },
                { "{{NombreVendedor}}", seller.Name.Trim() },
                { "{{CiudadVendedor}}", seller.City },
                { "{{DocumentoVendedor}}", seller.DocumentNumber },
                { "{{DocumentExpeditionVendedor}}", seller.DocumentExpedition },
                { "{{RolVendedor}}", seller.Job },
                { "{{CompanyVendedor}}", seller.Company },
                { "{{CompanyNitVendedor}}", seller.CompanyNit },
                { "{{nityourInvoice}}", nityourInvoice.Descripton },
                { "{{documentType}}", documentType.Descripton },
                { "{{tableContent}}", tableContent.ToString() }
            };
            
            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, iText.Kernel.Geom.PageSize.LETTER, null, false, false);
            return pdfTemplate;
        }

        private async Task<MemoryStream> GenerateMoneyTransferInstructionDocumentExcel(GenerateDocsCommand command)
        {
            var beneficiaries = await _moneyTransferRepository.ListToMoneyTransferDocumentAsync(command.offerId);
            DataTable dt = new DataTable("DataInvoice");
            dt.Columns.AddRange(new DataColumn[] { new DataColumn(beneficiaryName), new DataColumn(identificationType), new DataColumn(identificationNumber),
                new DataColumn(destinationFinancialInstitution), new DataColumn(destinationAccountNumber), new DataColumn(destinationAccountType)});
            beneficiaries.TableContent.ForEach(inv => dt.Rows.Add(inv.Name, inv.DocumentType, inv.DocumentNumber, inv.Bank, inv.AccountNumber, inv.AccountType));
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var sheet = workbook.AddWorksheet(dt, "Base de datos Beneficiarios");
                sheet.Columns(1, 7).Width = 25;
                MemoryStream ms = new();
                workbook.SaveAs(ms);
                return ms;
            }
        }

        private async Task<MemoryStream> GenerateCommercialOfferDocument(GenerateDocsCommand command, Domain.Offer offer, Domain.Users.User user, string date, bool isNatural, string responsability)
        {
            yourInvoice.Common.Entities.CatalogItemInfo template = null;

            if (isNatural)
                template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PfdCommercialOfferPn);
            else
                template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PfdCommercialOfferPj);

            var response = _invoiceRepository.ListToAppendix1Document(command.offerId);

            StringBuilder tableContent = new();

            foreach (var itemResponse in response)
            {
                // Agregar cadenas al StringBuilder
                tableContent.Append("<tr>");
                tableContent.Append("<td>" + itemResponse.InvoiceNumber + "</td>");
                tableContent.Append("<td>" + itemResponse.SellerName + "</td>");
                tableContent.Append("<td>" + itemResponse.PayerName + "</td>");
                tableContent.Append("<td>" + itemResponse.NitPayer + "</td>");
                tableContent.Append("<td>" + itemResponse.EmitDate + "</td>");
                tableContent.Append("<td>" + itemResponse.NegotiationDate + "</td>");
                tableContent.Append("<td>" + itemResponse.NegotiationTotal.ToString("C0", colombianCulture) + "</td>");
                tableContent.Append("</tr>");
            }

            var documentType = await _catalogBusinesss.GetByIdAsync(user.DocumentTypeId);

            Dictionary<string, string> replacements = new()
            {
                { "{{consecutive}}", offer.Consecutive.ToString() },
                { "{{date}}", date},
                { "{{legalRepresentativeName}}", user.Name.Trim() },
                { "{{representativeDocument}}", user.DocumentNumber },
                { "{{representativeDocumentExpedition}}", user.DocumentExpedition },
                { "{{legalRepresentativeCity}}", user.City },
                { "{{documentType}}", documentType.Name },
                { "{{conOsin}}", responsability },
                { "{{societyName}}", user.Company },
                { "{{societyNit}}", user.CompanyNit },
                { "{{commercialRegistrationNumber}}", user.CompanyCommercialRegistrationNumber },
                { "{{commercialRegistrationCity}}", user.CompanyCommercialRegistrationCity },
                { "{{chamberCommerceCity}}", user.CompanyChamberOfCommerceCity },
                { "{{nameSeller}}", user.Name.Trim() },
                { "{{addressSeller}}", user.Address },
                { "{{positionSeller}}", "Representante Legal" },
                { "{{phoneNumberSeller}}", user.Phone },
                { "{{emailSeller}}", user.Email.Trim() },
                { "{{citySeller}}", user.City },
                { "{{tableContent}}", tableContent.ToString() }
            };
            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, iText.Kernel.Geom.PageSize.LETTER, null, false, false);
            return pdfTemplate;
        }

        private async Task<MemoryStream> GenerateAppendix1Document(GenerateDocsCommand command, Domain.Users.User user)
        {
            var template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PdfAppendix1);

            var responseAppendix = _invoiceRepository.ListToAppendix1Document(command.offerId);

            StringBuilder tableContent = new();

            foreach (var itemResponseAppendix in responseAppendix)
            {
                // Agregar cadenas al StringBuilder
                tableContent.Append("<tr>");
                tableContent.Append("<td>" + itemResponseAppendix.InvoiceNumber + "</td>");
                tableContent.Append("<td>" + itemResponseAppendix.SellerName.Trim() + "</td>");
                tableContent.Append("<td>" + itemResponseAppendix.PayerName.Trim() + "</td>");
                tableContent.Append("<td>" + itemResponseAppendix.EmitDate + "</td>");
                tableContent.Append("<td>" + itemResponseAppendix.DueDate + "</td>");
                tableContent.Append("<td>" + itemResponseAppendix.NegotiationDate + "</td>");
                tableContent.Append("<td>" + itemResponseAppendix.NegotiationTotal.ToString("C", colombianCulture) + "</td>");
                tableContent.Append("</tr>");
            }

            var representativeName = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.RepresentativeLegalName);
            var representativeDocument = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.RepresentativeLegalDocument);

            Dictionary<string, string> replacements = new();

            replacements.Add("{{legalRepresentativeName}}", representativeName.Descripton);
            replacements.Add("{{document}}", representativeDocument.Descripton);
            replacements.Add("{{nameSeller}}", user.Name);
            replacements.Add("{{nit}}", user.DocumentNumber);
            replacements.Add("{{tableContent}}", tableContent.ToString());

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, iText.Kernel.Geom.PageSize.LETTER, null, false, false);
            return pdfTemplate;
        }

        private async Task<MemoryStream> GenerateEndorsementNotificationDocument(GenerateDocsCommand command, string city, string date, Domain.Users.User user, Payer payer, bool isNatural)
        {
            yourInvoice.Common.Entities.CatalogItemInfo template = null;

            if (isNatural)
                template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PdfEndorsementNotificationPn);
            else
                template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PdfEndorsementNotificationPj);

            var bank = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.Bank);
            var accountType = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.AccountType);
            var accountNumber = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.AccountNumber);
            var documentType = await _catalogBusinesss.GetByIdAsync(user.DocumentTypeId);
            var responseEndorsementNotification = _invoiceRepository.ListToAppendix1Document(command.offerId);

            StringBuilder tableContent = new();

            foreach (var itemResponse in responseEndorsementNotification)
            {
                // Agregar cadenas al StringBuilder
                tableContent.Append("<tr>");
                tableContent.Append("<td>" + itemResponse.InvoiceNumber + "</td>");
                tableContent.Append("<td>" + itemResponse.SellerName + "</td>");
                tableContent.Append("<td>" + payer.Nit + "</td>");
                tableContent.Append("<td>" + itemResponse.PayerName + "</td>");
                tableContent.Append("<td>" + itemResponse.EmitDate + "</td>");
                tableContent.Append("<td>" + itemResponse.NegotiationDate + "</td>");
                tableContent.Append("<td>" + itemResponse.NegotiationTotal.ToString("C0", colombianCulture) + "</td>");
                tableContent.Append("</tr>");
            }

            Dictionary<string, string> replacements = new()
            {
                { "{{ciudad}}", city },
                { "{{fecha}}", date },
                { "{{NombrePagador}}", payer.Name.Trim() },
                { "{{NitPagador}}", payer.Nit },
                { "{{direccionPagador}}", payer.Address },
                { "{{emailPagador}}", payer.Email },
                { "{{ciudadPagador}}", payer.City },
                { "{{NombreEmisor}}", user.Name },
                { "{{NITEmisor}}", user.CompanyNit },
                { "{{NombreRepresentanteLegal}}", user.Name.Trim() },
                { "{{representativeDocumentExpedition}}", user.DocumentExpedition },
                { "{{Documento}}", user.DocumentNumber },
                { "{{NombreEmpresaEmisor}}", user.Company },
                { "{{emailEmisor}}", user.Email },
                { "{{direccionEmisor}}", user.Address },
                { "{{telefonoEmisor}}", user.Phone },
                { "{{documentType}}", documentType.Name },
                { "{{Banco}}", bank.Descripton },
                { "{{TipoCuenta}}", accountType.Descripton },
                { "{{NumeroCuenta}}", accountNumber.Descripton },
                { "{{tableContent}}", tableContent.ToString() }
            };
            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, iText.Kernel.Geom.PageSize.LETTER, null, false, false);
            return pdfTemplate;
        }

        private async Task<MemoryStream> GenerateEndorsementDocument(GenerateDocsCommand command, string city, string date, Domain.Users.User user, Payer payer, bool isNatural, string responsability)
        {
            yourInvoice.Common.Entities.CatalogItemInfo template = null;

            if (isNatural)
                template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PdfEndorsementPn);
            else
                template = await _catalogBusinesss.GetByIdAsync(CatalogCode_Templates.PdfEndorsementPj);

            var nityourInvoice = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.Nit);
            var documentType = await _catalogBusinesss.GetByIdAsync(user.DocumentTypeId);
            var responseEndorsement = _invoiceRepository.ListToAppendix1Document(command.offerId);
            var bank = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.Bank);
            var accountType = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.AccountType);
            var accountNumber = await _catalogBusinesss.GetByIdAsync(CatalogCode_DatayourInvoice.AccountNumber);

            StringBuilder tableContents = new();

            foreach (var itemResponse in responseEndorsement)
            {
                // Agregar cadenas al StringBuilder
                tableContents.Append("<tr>");
                tableContents.Append("<td>" + itemResponse.InvoiceNumber + "</td>");
                tableContents.Append("<td>" + itemResponse.SellerName + "</td>");
                tableContents.Append("<td>" + payer.Nit + "</td>");
                tableContents.Append("<td>" + itemResponse.PayerName + "</td>");
                tableContents.Append("<td>" + itemResponse.EmitDate + "</td>");
                tableContents.Append("<td>" + itemResponse.NegotiationDate + "</td>");
                tableContents.Append("<td>" + itemResponse.NegotiationTotal.ToString("C0", colombianCulture) + "</td>");
                tableContents.Append("</tr>");
            }

            //EN LA PRIMERA VERSION SE AGREGABA UNA ULTIMA FILA PARA MOSTRAR EL TOTAL
            //var total = responseEndorsement.Sum(x => x.NegotiationTotal);
            //tableContents.Append("<tr style=\"background-color: white\">");
            //tableContents.Append("<td></td>");
            //tableContents.Append("<td style=\"font-weight: 700\">Total</td>");
            //tableContents.Append("<td style=\"font-weight: 700\">" + total.ToString("C0", colombianCulture) + "</td>");
            //tableContents.Append("<td></td>");
            //tableContents.Append("<td></td>");
            //tableContents.Append("</tr>");

            Dictionary<string, string> replacement = new()
            {
                { "{{ciudad}}", city },
                { "{{fecha}}", date },
                { "{{NombreRepresentanteLegal}}", user.Name.Trim() },
                { "{{representativeDocument}}", user.DocumentNumber },
                { "{{representativeDocumentExpedition}}", user.DocumentExpedition },
                { "{{NombreEmpresaEmisor}}", user.Company },
                { "{{UbicacionEmpresa}}", user.Address + ", " + user.City },
                { "{{NumeroNIT}}", user.CompanyNit },
                { "{{NombrePagador}}", payer.Name.Trim() },
                { "{{NIT_Pagador}}", payer.Nit },
                { "{{DomicilioPagador}}", payer.Address + ", " + payer.City },
                { "{{MailVendedor}}", user.Email },
                { "{{Documento}}", user.DocumentNumber },
                { "{{documentType}}", documentType.Name },
                { "{{conOsin}}", responsability },
                { "{{Banco}}", bank.Descripton },
                { "{{TipoCuenta}}", accountType.Descripton },
                { "{{NumeroCuenta}}", accountNumber.Descripton },
                { "{{nityourInvoice}}", nityourInvoice.Descripton },
                { "{{tableContent}}", tableContents.ToString() }
            };
            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacement);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, iText.Kernel.Geom.PageSize.LETTER, null, false, false);
            return pdfTemplate;
        }
    }
}