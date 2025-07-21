///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Admin.UploadAttachment
{
    public sealed class UploadAttachmentCommandHandler : IRequestHandler<UploadAttachmentCommand, ErrorOr<bool>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IStorage _storage;
        private readonly IUnitOfWork _unitOfWork;
        private const string Storage = "storage";
        private const string DocumentsOther = "Documents/Other";

        public UploadAttachmentCommandHandler(IDocumentRepository documentRepository, IStorage storage, IOfferRepository offerRepository, IUnitOfWork unitOfWork)
        {
            _documentRepository = documentRepository;
            _storage = storage;
            _offerRepository = offerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<bool>> Handle(UploadAttachmentCommand command, CancellationToken cancellationToken)
        {
            Domain.Offer offer = await _offerRepository.GetByConsecutiveAsync(command.OfferId);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            if (!IsSupportedFile(command.FileName))
                return Error.Validation(MessageCodes.FileRejectByNoZip, GetErrorDescription(MessageCodes.FileRejectByNoZip));

            byte[] file = Convert.FromBase64String(command.FileBase64);

            var currentFileSize = (file.Length / Math.Pow(1024, 2));

            if (currentFileSize > 5)
            {
                return Error.Validation(MessageCodes.FileRejectByNoZip, GetErrorDescription(MessageCodes.FileSize5));
            }

            string storageRute = $"{Storage} / {offer.Consecutive} / {DocumentsOther}/ ";
            object urlFile = await _storage.UploadAsync(file, storageRute + command.FileName);

            Document doc = new(Guid.NewGuid(), offer.Id, null, command.FileName,
            CatalogCode_DocumentType.DocumentsUploadByUserOnResume, false, urlFile.ToString(), currentFileSize.ToMegaByte());
            _documentRepository.Add(doc);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static bool IsSupportedFile(string fileName)
        {
            // Obtener la extensión del archivo (sin considerar mayúsculas o minúsculas)
            string fileExtension = Path.GetExtension(fileName)?.ToLower();

            // Verificar si la extensión es la de un archivo PDF, PNG o JPG
            return fileExtension == ".pdf" || fileExtension == ".xml" || fileExtension == ".xlsx" || fileExtension == ".xls";
        }

    }
}