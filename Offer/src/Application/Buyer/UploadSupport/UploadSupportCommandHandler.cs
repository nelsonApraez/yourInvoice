///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Offer.Application.Buyer.UploadSupport
{
    public sealed class UploadSupportCommandHandler : IRequestHandler<UploadSupportCommand, ErrorOr<bool>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IStorage _storage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystem system;
        private const string Storage = "storage";
        private const string DocumentsBuyer = "Documents/Buyer";
        private const string SoporteGiroName = "Soporte de giro";

        public UploadSupportCommandHandler(IDocumentRepository documentRepository, IStorage storage, IOfferRepository offerRepository, IUnitOfWork unitOfWork, ISystem system)
        {
            _documentRepository = documentRepository;
            _storage = storage;
            _offerRepository = offerRepository;
            _unitOfWork = unitOfWork;
            this.system = system;
        }

        public async Task<ErrorOr<bool>> Handle(UploadSupportCommand command, CancellationToken cancellationToken)
        {
            var userId = this.system.User.Id;
            Domain.Offer offer = await _offerRepository.GetByIdAsync(command.OfferId);
            if (offer == null)
                return Error.Validation(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));

            if (!IsSupportedFile(command.FileName))
                return Error.Validation(MessageCodes.FileRejectByNoZip, GetErrorDescription(MessageCodes.FileRejectByNoZip));

            string fileExtension = Path.GetExtension(command.FileName)?.ToLower();

            byte[] file = Convert.FromBase64String(command.FileBase64);

            string storageRute = $"{Storage}/{offer.Consecutive}/{DocumentsBuyer}/";
            object urlFile = await _storage.UploadAsync(file, storageRute + SoporteGiroName + fileExtension);

            var documents = await _documentRepository.GetAllDocumentsByOfferAsync(command.OfferId);

            //si ya existe el documento en la tabla se consulta y se actualiza
            var fileInBD = documents.Where(x => x.TypeId == CatalogCode_DocumentType.TransferSupportBuyer && x.RelatedId == userId).FirstOrDefault();

            if (fileInBD == null)
            {
                Document doc = new(Guid.NewGuid(), command.OfferId, userId, SoporteGiroName + fileExtension,
                CatalogCode_DocumentType.TransferSupportBuyer, false, urlFile.ToString(), file.Length.ToMegaByte());
                doc.CreatedBy = userId;
                _documentRepository.Add(doc);
            }
            else
            {
                fileInBD.FileSize = file.Length.ToMegaByte();
                fileInBD.CreatedBy = userId;
                _documentRepository.Update(fileInBD);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static bool IsSupportedFile(string fileName)
        {
            // Obtener la extensión del archivo (sin considerar mayúsculas o minúsculas)
            string fileExtension = Path.GetExtension(fileName)?.ToLower();

            // Verificar si la extensión es la de un archivo PDF, PNG o JPG
            return fileExtension == ".pdf" || fileExtension == ".png" || fileExtension == ".jpg";
        }
    }
}