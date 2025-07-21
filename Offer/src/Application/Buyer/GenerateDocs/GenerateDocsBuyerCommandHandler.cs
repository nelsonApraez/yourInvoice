///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using SelectPdf;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.PdfModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using System.Globalization;
using System.Text;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.GenerateDocs
{
    public sealed class GenerateDocsBuyerCommandHandler : IRequestHandler<GenerateDocsBuyerCommand, ErrorOr<bool>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMoneyTransferRepository _moneyTransferRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceDispersionRepository _invoiceDispersionRepository;
        private readonly ISystem system;
        private readonly IUserRepository _userRepository;
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IPayerRepository _payerRepository;
        private readonly IUnitOfWork _unitOfWork;

        private const string Storage = "storage";
        private const string DocumentsBuyer = "Documents/Buyer";
        private const string CertificadoCompraName = "Certificacion de Compra.pdf";
        private const string InstruccionGiroName = "Instruccion de Giro Inversionista.pdf";
        private const string OfertaMercantilAceptadaName = "Aceptacion Oferta Mercantil e Instruccion de giro.pdf";

        private CultureInfo colombianCulture = new CultureInfo("es-CO");

        public GenerateDocsBuyerCommandHandler(IUnitOfWork unitOfWork, IStorage storage, IOfferRepository offerRepository,
            IMoneyTransferRepository moneyTransferRepository, IUserRepository userRepository, IDocumentRepository documentRepository,
            IInvoiceRepository invoiceRepository, ICatalogBusiness catalogBusiness, IPayerRepository payerRepository, IInvoiceDispersionRepository invoiceDispersionRepository,
            ISystem system)
        {
            this._moneyTransferRepository = moneyTransferRepository ?? throw new ArgumentNullException(nameof(moneyTransferRepository));
            this._offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            _payerRepository = payerRepository ?? throw new ArgumentNullException(nameof(payerRepository));
            _invoiceDispersionRepository = invoiceDispersionRepository ?? throw new ArgumentNullException(nameof(invoiceDispersionRepository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public async Task<ErrorOr<bool>> Handle(GenerateDocsBuyerCommand command, CancellationToken cancellationToken)
        {
            var buyerId = this.system.User.Id;

            Domain.Offer offer = await _offerRepository.GetByConsecutiveAsync(command.numberOffer);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            //si el estado de la oferta es comprada saca error
            bool IsPurchased = await _offerRepository.OfferIsPurchasedAsync(command.numberOffer);
            if (IsPurchased)
                return Error.Validation(MessageCodes.MessageOfferIsPurchased, GetErrorDescription(MessageCodes.MessageOfferIsPurchased));

            var invoiceDispersion = await _invoiceDispersionRepository.GetByOfferNumberAndBuyerIdAsync(command.numberOffer, buyerId);

            if (invoiceDispersion == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            //validar si los documentos ya habian sido generados previamente y si ya estan firmados por el mismo comprador.
            var documents = await _documentRepository.GetAllDocumentsByOfferAsync(offer.Id);
            var anyIsSigned = documents.Any(x => x.TypeId == CatalogCode_DocumentType.CommercialOfferBuyer && x.IsSigned == true && x.RelatedId == buyerId);

            if (anyIsSigned)
                return Error.Validation(MessageCodes.DocumentIsSigned, GetErrorDescription(MessageCodes.DocumentIsSigned));

            var payer = await _payerRepository.GetByIdAsync(invoiceDispersion.PayerId);
            var buyer = await _userRepository.GetByIdAsync(invoiceDispersion.BuyerId);
            var seller = await _userRepository.GetByIdAsync(invoiceDispersion.SellerId);
            Guid? personType = await _userRepository.GetPersonTypeByIdAsync(invoiceDispersion.BuyerId);
            string date = ExtensionFormat.DateTimeCO().ToString("d 'de' MMMM yyyy", new CultureInfo("es-ES", false));
            string city = buyer.City;

            bool isNatural = false;
            if (personType == CatalogCode_PersonType.Natural)
                isNatural = true;

            MemoryStream pdfCommercialOfferBuyer = await GenerateCommercialOfferBuyerDocument(offer, seller, buyer, city, date, isNatural);

            MemoryStream pdfPurchaseCertificate = await GeneratePurchaseCertificateDocument(offer.Consecutive, seller, buyer);
            MemoryStream pdfMoneyTransferInstructionBuyer = null;
            object urlInstruccionGiroComprador = null;
            string storageRute = $"{Storage}/{offer.Consecutive}/{DocumentsBuyer}/{buyer.DocumentNumber}/";

            bool IsNotNewMoneyOrMixed = _invoiceDispersionRepository.IsNotNewMoneyOrMixed(buyer.Id, offer.Consecutive);

            if (IsNotNewMoneyOrMixed)
            {
                pdfMoneyTransferInstructionBuyer = await GenerateMoneyTransferInstructionBuyerDocument(offer.Consecutive, seller, buyer, date, payer, isNatural, offer.Id, city);
                urlInstruccionGiroComprador = await _storage.UploadAsync(pdfMoneyTransferInstructionBuyer.ToArray(), storageRute + InstruccionGiroName);
            }

            object urlCertificadoCompra = await _storage.UploadAsync(pdfPurchaseCertificate.ToArray(), storageRute + CertificadoCompraName);
            object urlOfertaMercantilAceptada = await _storage.UploadAsync(pdfCommercialOfferBuyer.ToArray(), storageRute + OfertaMercantilAceptadaName);

            await SaveInDB(offer.Id, urlCertificadoCompra, urlOfertaMercantilAceptada, urlInstruccionGiroComprador, cancellationToken, 
                pdfPurchaseCertificate.Length, pdfCommercialOfferBuyer.ToArray().LongLength, pdfMoneyTransferInstructionBuyer, documents, invoiceDispersion.BuyerId);

            return true;
        }

        private async Task SaveInDB(Guid offerId, object urlCertificadoCompra, object urlOfertaMercantilAceptada,
            object urlInstruccionGiroComprador, CancellationToken cancellationToken,
            long sizePurchaseCertificate, long sizeCommercialOfferBuyer, MemoryStream sizeMtiBuyer, List<Domain.Documents.Document> documents, Guid buyerId)
        {
            //si ya existe el documento en la tabla se consulta y se actualiza
            var purchaseCertificate = documents.Where(x => x.TypeId == CatalogCode_DocumentType.PurchaseCertificate && x.RelatedId == buyerId).FirstOrDefault();
            var moneyTransferInstructionBuyer = documents.Where(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstructionBuyer && x.RelatedId == buyerId).FirstOrDefault();
            var commercialOffer = documents.Where(x => x.TypeId == CatalogCode_DocumentType.CommercialOfferBuyer && x.RelatedId == buyerId).FirstOrDefault();

            Domain.Documents.Document CertificadoCompra = new(Guid.NewGuid(), offerId, buyerId, CertificadoCompraName, CatalogCode_DocumentType.PurchaseCertificate, false, urlCertificadoCompra.ToString(), sizePurchaseCertificate.ToMegaByte());
            Domain.Documents.Document OfertaMercantilAceptada = new(Guid.NewGuid(), offerId, buyerId, OfertaMercantilAceptadaName, CatalogCode_DocumentType.CommercialOfferBuyer, false, urlOfertaMercantilAceptada.ToString(), (sizeCommercialOfferBuyer * 10).ToMegaByte());

            Domain.Documents.Document InstruccionGiroComprador = null;
            if (urlInstruccionGiroComprador != null)
            {
                InstruccionGiroComprador = new(Guid.NewGuid(), offerId, buyerId, InstruccionGiroName,
                    CatalogCode_DocumentType.MoneyTransferInstructionBuyer, false, urlInstruccionGiroComprador.ToString(), (sizeMtiBuyer.ToArray().LongLength * 10).ToMegaByte());
            }

            if (purchaseCertificate == null)
            {
                _documentRepository.Add(CertificadoCompra);
                _documentRepository.Add(OfertaMercantilAceptada);

                if (urlInstruccionGiroComprador != null)
                {
                    _documentRepository.Add(InstruccionGiroComprador);
                }
            }
            else
            {
                purchaseCertificate.FileSize = sizePurchaseCertificate.ToMegaByte();
                commercialOffer.FileSize = (sizeCommercialOfferBuyer * 10).ToMegaByte();
                _documentRepository.Update(purchaseCertificate);
                _documentRepository.Update(commercialOffer);

                if (urlInstruccionGiroComprador != null)
                {
                    moneyTransferInstructionBuyer.FileSize = (sizeMtiBuyer.ToArray().LongLength * 10).ToMegaByte();
                    _documentRepository.Update(moneyTransferInstructionBuyer);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<MemoryStream> GenerateMoneyTransferInstructionBuyerDocument(int offerNumber, Domain.Users.User seller, Domain.Users.User buyer,
            string date, Payer payer, bool isNatural, Guid offerId, string city)
        {
            yourInvoice.Common.Entities.CatalogItemInfo template = null;

            if (isNatural)
                template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.PdfMoneyTransferInstructionBuyerPn);
            else
                template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.PdfMoneyTransferInstructionBuyerPj);

            var responseMoneyTrans = await _moneyTransferRepository.ListToMoneyTransferDocumentAsync(offerId);

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

            var nityourInvoice = await _catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.Nit);
            var documentType = await _catalogBusiness.GetByIdAsync(buyer.DocumentTypeId);
            long valor = _invoiceDispersionRepository.GetTotalPurchased(buyer.Id, offerNumber);

            Dictionary<string, string> replacements = new()
            {
                { "{{ciudad}}", city },
                { "{{fecha}}", date },
                { "{{nameBuyer}}", buyer.Name.Trim() },
                { "{{documentNumberBuyer}}", buyer.DocumentNumber },
                { "{{addressBuyer}}", buyer.Address },
                { "{{emailBuyer}}", buyer.Email },
                { "{{consecutive}}", offerNumber.ToString() },
                { "{{NombrePagador}}", payer.Name.Trim() },
                { "{{NombreVendedor}}", seller.Name.Trim() },
                { "{{CiudadVendedor}}", seller.City },
                { "{{DocumentoVendedor}}", seller.DocumentNumber },
                { "{{DocumentExpeditionVendedor}}", seller.DocumentExpedition },
                { "{{RolVendedor}}", "Vendedor" },
                { "{{CompanyVendedor}}", seller.Company },
                { "{{CompanyNitVendedor}}", seller.CompanyNit },
                { "{{MatriculaMercantilVendedor}}", seller.CompanyCommercialRegistrationNumber },
                { "{{nityourInvoice}}", nityourInvoice.Descripton },
                { "{{documentType}}", documentType.Descripton },
                { "{{Valor}}", valor.ToString("C0", colombianCulture) },
                { "{{tableContent}}", tableContent.ToString() }
            };

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, iText.Kernel.Geom.PageSize.LETTER, null, false, false); 
            return pdfTemplate;
        }

        private async Task<MemoryStream> GenerateCommercialOfferBuyerDocument(Domain.Offer offer, Domain.Users.User seller, Domain.Users.User buyer,
            string city, string date, bool isNatural)
        {
            yourInvoice.Common.Entities.CatalogItemInfo template = null;

            if (isNatural)
                template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.PfdCommercialOfferAcceptedPn);
            else
                template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.PfdCommercialOfferAcceptedPj);

            var documentType = await _catalogBusiness.GetByIdAsync(buyer.DocumentTypeId);

            Dictionary<string, string> replacements = new()
            {
                { "{{ciudad}}", city },
                { "{{fecha}}", date },
                { "{{consecutive}}", offer.Consecutive.ToString() },
                { "{{documentType}}", documentType.Descripton },
                { "{{societyName}}", seller.Company },
                { "{{societyNit}}", seller.CompanyNit },
                { "{{NombreVendedor}}", seller.Name.Trim() },
                { "{{MatriculaMercantil}}", buyer.CompanyCommercialRegistrationNumber },
                { "{{commercialRegistrationCity}}", buyer.CompanyCommercialRegistrationCity },
                { "{{chamberCommerceCity}}", buyer.CompanyChamberOfCommerceCity },
                { "{{fechaVenta}}", offer.CreatedOn.Value.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES", false)) },
                { "{{nameBuyer}}", buyer.Name.Trim() },
                { "{{cargo}}", buyer.Job },
                { "{{documentNumberBuyer}}", buyer.DocumentNumber },
                { "{{cityBuyer}}", buyer.City },
                { "{{NombreEmpresa}}", buyer.Company },
                { "{{emailBuyer}}", buyer.Email },
                { "{{addressBuyer}}", buyer.Address },
                { "{{phoneBuyer}}", buyer.Phone },
                { "{{nitEmpresa}}", buyer.CompanyNit }
            };

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);

            MemoryStream pdfTemplate = PDFTableBusiness.HtmlToPdf(templateWithData, iText.Kernel.Geom.PageSize.LETTER, null, false, false);
            return pdfTemplate;
        }

        private async Task<MemoryStream> GeneratePurchaseCertificateDocument(int offerNumber, Domain.Users.User seller, Domain.Users.User buyer)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("es-CO");
            var template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.PdfPurchaseCertificate);

            var responsePurchaseCertificate = await _invoiceDispersionRepository.ListToPurchaseCertificateDocument(buyer.Id, offerNumber);

            StringBuilder tableContent = new();

            foreach (var itemPurchaseCertificate in responsePurchaseCertificate)
            {
                // Agregar cadenas al StringBuilder
                tableContent.Append("<tr>");
                tableContent.Append("<td>" + itemPurchaseCertificate.CreationDate + "</td>");
                tableContent.Append("<td>" + itemPurchaseCertificate.EndDate + "</td>");
                tableContent.Append("<td>" + itemPurchaseCertificate.TransactionNumber + "</td>");
                tableContent.Append("<td>" + "Fac" + "</td>");
                tableContent.Append("<td>" + itemPurchaseCertificate.InvoiceNumber + "</td>");
                tableContent.Append("<td>" + itemPurchaseCertificate.PayerName + "</td>");
                tableContent.Append("<td>" + itemPurchaseCertificate.SellerName + "</td>");
                tableContent.Append("<td>" + itemPurchaseCertificate.Rate + "</td>");
                tableContent.Append("<td>" + $"{Convert.ToInt64(itemPurchaseCertificate.PurchaseValue):C0}" + "</td>");
                tableContent.Append("<td>" + itemPurchaseCertificate.Term + "</td>");
                tableContent.Append("<td>" + $"{Convert.ToInt64(itemPurchaseCertificate.FutureValue):C0}" + "</td>");
                tableContent.Append("</tr>");
            }

            var total = responsePurchaseCertificate.Sum(x => Convert.ToInt64(x.PurchaseValue));
            var totalFuture = responsePurchaseCertificate.Sum(x => Convert.ToInt64(x.FutureValue));
            var documentType = await _catalogBusiness.GetByIdAsync(buyer.DocumentTypeId);

            Dictionary<string, string> replacements = new()
            {
                { "{{Nombre}}", buyer.Name.Trim() },
                { "{{NumeroCedula}}", buyer.DocumentNumber },
                { "{{NumeroContacto}}", buyer.Phone },
                { "{{Direccion}}", buyer.Address },
                { "{{NombreVendedor}}", seller.Company },
                { "{{total}}", total.ToString("C0", colombianCulture) },
                { "{{totalFuture}}", totalFuture.ToString("C0", colombianCulture) },
                { "{{DocumentType}}", documentType.Descripton },
                { "{{tableContent}}", tableContent.ToString() }
            };

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);

            MemoryStream pdfTemplate = PDFBusiness.HtmlToPdf(templateWithData, PdfPageSize.Letter, PdfPageOrientation.Landscape, 1100, 0, 0, 32, 0, 24);
            return pdfTemplate;
        }
    }
}