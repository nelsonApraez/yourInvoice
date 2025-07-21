///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Offer.Application.Buyer.EmailToAdmin;
using yourInvoice.Offer.Application.HistoricalStates.Add;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using System.Data;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.SignSuccessDocs
{
    public sealed class SignSuccessDocsBuyerCommandHandler : IRequestHandler<SignSuccessDocsBuyerCommand, ErrorOr<bool>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IZapsign _Zapsign;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorage _storage;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator mediator;
        private readonly IInvoiceDispersionRepository _invoiceDispersionRepository;
        private readonly ISystem system;
        private const string CertificadoCompraSigned = "Certificacion de Compra Firmado.pdf";
        private const string OfertaMercantilAceptadaSigned = "Aceptacion Oferta Mercantil e Instruccion de giro Firmado.pdf";
        private const string InstruccionGiroInversionistaSigned = "Instruccion de Giro Inversionista Firmado.pdf";
        private const string InstruccionGiroInversionistaDoc = "Instruccion de Giro Inversionista.pdf";
        private const string CertificadoCompraDoc = "Certificacion de Compra.pdf";
        private const string Storage = "storage";
        private const string DocumentsBuyer = "Documents/Buyer";

        public SignSuccessDocsBuyerCommandHandler(IUnitOfWork unitOfWork, IStorage storage, IOfferRepository offerRepository,
            IZapsign zapsign, IUserRepository userRepository, IDocumentRepository documentRepository, ICatalogBusiness catalogBusiness,
            IMediator mediator, IInvoiceDispersionRepository invoiceDispersionRepository, ISystem system)
        {
            _Zapsign = zapsign ?? throw new ArgumentNullException(nameof(zapsign));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            this.mediator = mediator;
            _invoiceDispersionRepository = invoiceDispersionRepository;
            this.system = system;
        }

        public async Task<ErrorOr<bool>> Handle(SignSuccessDocsBuyerCommand command, CancellationToken cancellationToken)
        {
            string certificadoCompraUrlSignedFile = null;
            string instruccionGiroInversionistaUrlSignedFile = null;
            object urlInstruccionGiroInversionista = null;

            var buyerId = this.system.User.Id;
            Domain.Offer offer = await _offerRepository.GetByConsecutiveAsync(command.numberOffer);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var invoiceDispersion = await _invoiceDispersionRepository.GetByOfferNumberAndBuyerIdAsync(command.numberOffer, buyerId);

            if (invoiceDispersion == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            var documents = await _documentRepository.GetAllDocumentsByOfferAsync(offer.Id);

            var docCommercialOfferBuyer = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.CommercialOfferBuyer && x.RelatedId == buyerId);
            var docPurchaseCertificate = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.PurchaseCertificate && x.RelatedId == buyerId);
            var docMoneyTransferInstructionBuyer = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.MoneyTransferInstructionBuyer && x.RelatedId == buyerId);

            var containerName = await _catalogBusiness.GetByIdAsync(CatalogCode_Storage.ContainerName);

            var userBuyer = await _userRepository.GetByIdAsync(invoiceDispersion.BuyerId);

            if (string.IsNullOrEmpty(docCommercialOfferBuyer.TokenZapsign))
            {
                return Error.Validation(MessageCodes.ZapsignNoToken, GetErrorDescription(MessageCodes.ZapsignNoToken));
            }

            //Obtener los documentos firmados desde zapsign
            var zapsignResponse = await _Zapsign.GetDetailAsync(docCommercialOfferBuyer.TokenZapsign);

            if (zapsignResponse.status == "pending")
            {
                return Error.Validation(MessageCodes.ZapsignError, GetErrorDescription(MessageCodes.ZapsignError));
            }

            //si hay algun anexo que no traiga el documento firmado se intenta traer con el token de cada uno. De lo contrario se extraen del documento principal
            if (zapsignResponse.extra_docs.Any(x => x.signed_file == null))
            {
                //Intentar obtener los documentos anexados firmados
                if (docMoneyTransferInstructionBuyer != null)
                {
                    var instruccionGiroInversionista = await _Zapsign.GetDetailAsync(zapsignResponse.extra_docs.Where(x => x.name == InstruccionGiroInversionistaDoc).FirstOrDefault().token);
                    if (instruccionGiroInversionista == null || instruccionGiroInversionista.signed_file == null)
                    {
                        return Error.Validation(MessageCodes.ZapsignError, GetErrorDescription(MessageCodes.ZapsignError));
                    }
                    else
                    {
                        instruccionGiroInversionistaUrlSignedFile = instruccionGiroInversionista.signed_file;
                    }
                }

                var certificadoCompra = await _Zapsign.GetDetailAsync(zapsignResponse.extra_docs.Where(x => x.name == CertificadoCompraDoc).FirstOrDefault().token);
                if (certificadoCompra == null || certificadoCompra.signed_file == null)
                {
                    return Error.Validation(MessageCodes.ZapsignError, GetErrorDescription(MessageCodes.ZapsignError));
                }
                else
                {
                    certificadoCompraUrlSignedFile = certificadoCompra.signed_file;
                }
            }
            else
            {
                //se extraen del documento principal
                certificadoCompraUrlSignedFile = zapsignResponse.extra_docs.Where(x => x.name == CertificadoCompraDoc).FirstOrDefault().signed_file;
                if (docMoneyTransferInstructionBuyer != null)
                {
                    instruccionGiroInversionistaUrlSignedFile = zapsignResponse.extra_docs.Where(x => x.name == InstruccionGiroInversionistaDoc).FirstOrDefault().signed_file;
                }
            }

            //almacenar en el storage los documentos firmados
            string storageRute = $"{Storage}/{offer.Consecutive}/{DocumentsBuyer}/{userBuyer.DocumentNumber}/";

            if (docMoneyTransferInstructionBuyer != null)
            {
                urlInstruccionGiroInversionista = await _storage.UploadAsync(await DownloadFileAsync(instruccionGiroInversionistaUrlSignedFile), storageRute + InstruccionGiroInversionistaSigned);
            }
            object urlCertificadoCompra = await _storage.UploadAsync(await DownloadFileAsync(certificadoCompraUrlSignedFile), storageRute + CertificadoCompraSigned);
            object urlOfertaMercantilAceptada = await _storage.UploadAsync(await DownloadFileAsync(zapsignResponse.signed_file), storageRute + OfertaMercantilAceptadaSigned);

            //se actualizan los documentos
            await SaveInDB(docCommercialOfferBuyer, docPurchaseCertificate, docMoneyTransferInstructionBuyer, urlCertificadoCompra, urlOfertaMercantilAceptada, urlInstruccionGiroInversionista,
            cancellationToken);

            //Se actualiza el estado de las transacciones.
            var invoiceDispersions = await _invoiceDispersionRepository.FindByOfferNumberAndBuyerIdAsync(command.numberOffer, buyerId);
            await _invoiceDispersionRepository.ChangeStatusInvoiceDispersionPurchasedAsync(invoiceDispersions);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //Se actualiza el estado de la oferta.
            if (await _invoiceDispersionRepository.GetPurchasePercentageAsync(command.numberOffer) == 100)
            {
                offer.StatusId = CatalogCode_OfferStatus.Purchased;
                _offerRepository.Update(offer);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await this.mediator.Publish(new AddHistoricalCommand { OfferId = offer.Id, StatusId = CatalogCode_OfferStatus.Purchased, UserId = offer.UserId }, cancellationToken);
            }

            await SendEmails(offer, userBuyer);

            return true;
        }

        private async Task SendEmails(Domain.Offer offer, Domain.Users.User userBuyer)
        {
            //admin
            await this.mediator.Publish(new EmailToAdminBuyerApproveCommand { NumberOffer = offer.Consecutive, NameBuyer = userBuyer.Name });
        }

        private async Task SaveInDB(Document docCommercialOfferBuyer, Document docPurchaseCertificate, Document docMoneyTransferInstructionBuyer, object urlCertificadoCompra,
            object urlOfertaMercantilAceptada, object urlInstruccionGiroInversionista, CancellationToken cancellationToken)
        {
            if (docMoneyTransferInstructionBuyer != null)
            {
                docMoneyTransferInstructionBuyer.IsSigned = true;
                docMoneyTransferInstructionBuyer.Url = urlInstruccionGiroInversionista.ToString();
                docMoneyTransferInstructionBuyer.Name = InstruccionGiroInversionistaSigned;
                _documentRepository.Update(docMoneyTransferInstructionBuyer);
            }

            docPurchaseCertificate.IsSigned = true;
            docPurchaseCertificate.Url = urlCertificadoCompra.ToString();
            docPurchaseCertificate.Name = CertificadoCompraSigned;
            docCommercialOfferBuyer.IsSigned = true;
            docCommercialOfferBuyer.Url = urlOfertaMercantilAceptada.ToString();
            docCommercialOfferBuyer.Name = OfertaMercantilAceptadaSigned;

            //Se actualizan documentos
            _documentRepository.Update(docPurchaseCertificate);
            _documentRepository.Update(docCommercialOfferBuyer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
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