///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Admin.EmailToSeller;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using System.Text.RegularExpressions;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Admin.SendSummary
{
    public sealed class SendSummaryQueryHandler : IRequestHandler<SendSummaryQuery, ErrorOr<bool>>
    {
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly IStorage storage;
        private readonly IMediator mediator;

        public SendSummaryQueryHandler(IInvoiceDispersionRepository invoiceDispersionRepository, IDocumentRepository documentRepository, IStorage storage, IMediator mediator)
        {
            this.invoiceDispersionRepository = invoiceDispersionRepository;
            this.documentRepository = documentRepository;
            this.storage = storage;
            this.mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(SendSummaryQuery query, CancellationToken cancellationToken)
        {
            var documnetsResume = await this.documentRepository.GetDocumentsByOfferNumberAndTypeAsync(query.offerId, CatalogCode_DocumentType.DocumentsUploadByUserOnResume);
            if (documnetsResume is null || documnetsResume.Count <= 0)
            {
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));
            }
            var pathStorageFiles = documnetsResume.Select(s => GetPathStorage(s.Url)).ToList();

            var attachFile = new List<AttachFile>();
            foreach (var pathStorage in pathStorageFiles)
            {
                var file = await this.storage.DownloadAsync(DecodePercentEncoding(pathStorage));
                attachFile.Add(new AttachFile { File = file, PathFileWithExtension = DecodePercentEncoding(pathStorage) });
            }
            var nameCompanySeller = await this.invoiceDispersionRepository.GetNameCompanySellerByOfferAsync(query.offerId);
            var emailToSeller = new EmailToSellerAdminPurchasedCommand
            {
                AttachFilesData = attachFile,
                NameSeller = nameCompanySeller,
                NumberOffer = query.offerId,
                EmailsSeller = query.emailsSeller,
            };

            await this.mediator.Publish(emailToSeller, cancellationToken);
            return true;
        }

        static string DecodePercentEncoding(string encodedString)
        {
            // Utilizar expresiones regulares para buscar y reemplazar todos los caracteres codificados
            string pattern = @"%([0-9A-Fa-f]{2})";
            string decodedString = Regex.Replace(encodedString, pattern, match =>
            {
                // Obtener el valor hexadecimal y convertirlo a un carácter
                string hexValue = match.Groups[1].Value;
                int asciiValue = Convert.ToInt32(hexValue, 16);
                return ((char)asciiValue).ToString();
            });

            return decodedString;
        }

        private static string GetPathStorage(string path)
        {
            string pathMain = "storage";
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            if (!path.Contains("http"))
            {
                return path;
            }
            var pathTemp = path.Split(pathMain);
            return pathMain + pathTemp[1].ToString();
        }
    }
}