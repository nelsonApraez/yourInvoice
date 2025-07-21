///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalBoardDirector
{
    public sealed class CreateLegalBoardDirectorCommandHandler : IRequestHandler<CreateLegalBoardDirectorCommand, ErrorOr<bool>>
    {
        private readonly ILegalBoardDirectorRepository _repository;
        private readonly IUnitOfWorkLink _unitOfWorkLink;

        public CreateLegalBoardDirectorCommandHandler(ILegalBoardDirectorRepository repository, IUnitOfWorkLink unitOfWorkLink)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWorkLink = unitOfWorkLink ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(CreateLegalBoardDirectorCommand request, CancellationToken cancellationToken)
        {
            LegalBoardDirector entity = new LegalBoardDirector(
                Guid.NewGuid(),
                request.id_LegalGeneralInformation,
                request.fullNameCompanyName,
                request.documentTypeId,
                request.documentNumber,
                request.phoneNumber,
                request.completed,
                Guid.Empty,
                null,
                true,
                ExtensionFormat.DateTimeCO(),
                request.id_LegalGeneralInformation,
                null,
                Guid.Empty);

            _repository.AddLegalBoardDirector(entity);

            await _unitOfWorkLink.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
