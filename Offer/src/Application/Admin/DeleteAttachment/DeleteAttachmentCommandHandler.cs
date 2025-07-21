///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Application.Admin.DeleteAttachment
{
    public sealed class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, ErrorOr<bool>>
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IStorage storage;
        private readonly IUnitOfWork unitOfWork;

        public DeleteAttachmentCommandHandler(IDocumentRepository documentRepository, IStorage storage, IUnitOfWork unitOfWork)
        {
            this.documentRepository = documentRepository;
            this.storage = storage;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<bool>> Handle(DeleteAttachmentCommand command, CancellationToken cancellationToken)
        {
            var document = await this.documentRepository.GetByIdAsync(command.documentId);
            if (document is null || document.Count <= 0)
            {
                return false;
            }
            await this.documentRepository.DeleteAsync(command.documentId);
            var pathStorageFile = document.FirstOrDefault()?.Url;
            await this.storage.DeleteBlobByUrlAsync(pathStorageFile);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}