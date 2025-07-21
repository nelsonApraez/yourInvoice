///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;

namespace yourInvoice.Link.Application.LinkingProcess.DeleteLegalBoardDirector
{

    public sealed class DeleteLegalBoardDirectorCommandHandler : IRequestHandler<DeleteLegalBoardDirectorCommand, ErrorOr<bool>>
    {
        private readonly ILegalBoardDirectorRepository _repository;

        public DeleteLegalBoardDirectorCommandHandler(ILegalBoardDirectorRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(DeleteLegalBoardDirectorCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsLegalBoardDirectorById(request.Id, request.Id_LegalGeneralInformation))
            {
                await _repository.DeleteLegalBoardDirector(request.Id, request.Id_LegalGeneralInformation);
            }

            return true;
        }
    }
}
