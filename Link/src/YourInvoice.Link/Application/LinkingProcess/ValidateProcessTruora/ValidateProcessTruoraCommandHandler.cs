using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Integration.Truora;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.ValidateProcessTruora
{
    public sealed class ValidateProcessTruoraCommandHandler : IRequestHandler<ValidateProcessTruoraCommand, ErrorOr<bool>>
    {
        private readonly ITruora _truora;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWorkLink _unitOfWork;

        public ValidateProcessTruoraCommandHandler(ITruora truora, IDocumentRepository documentRepository, IUnitOfWorkLink unitOfWork)
        {
            _truora = truora ?? throw new ArgumentNullException(nameof(truora));
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<bool>> Handle(ValidateProcessTruoraCommand command, CancellationToken cancellationToken)
        {
            var documents = await _documentRepository.GetAllDocumentsByRelatedIdAsync(command.generalInformationId);
            var docLinkingFormat = documents.FirstOrDefault(x => x.TypeId == CatalogCode_DocumentType.LinkingFormat && x.RelatedId == command.generalInformationId);

            if (docLinkingFormat.IsSigned != null && docLinkingFormat.IsSigned == true)
                return Error.Validation(MessageCodes.DocumentsAreSigned, GetErrorDescription(MessageCodes.DocumentsAreSigned));

            await Task.Delay(TimeSpan.FromSeconds(1));// Se da 1 segundos mas para consultar
            var result = await _truora.GetProcessAsync(command.processId);
            
            if (result.Status == "success")
            {
                //se actualiza el documento principal con el process id generado
                docLinkingFormat.ProcessIdTruora = command.processId;
                _documentRepository.Update(docLinkingFormat);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                docLinkingFormat.ProcessIdTruora = command.processId;
                _documentRepository.Update(docLinkingFormat);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return false;
            }
        }
    }
}
