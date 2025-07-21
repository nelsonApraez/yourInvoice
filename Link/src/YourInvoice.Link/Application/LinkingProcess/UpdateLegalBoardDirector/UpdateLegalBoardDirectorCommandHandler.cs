///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalBoardDirector
{
    public class UpdateLegalBoardDirectorCommandHandler : IRequestHandler<UpdateLegalBoardDirectorCommand, ErrorOr<bool>>
    {
        private readonly ILegalBoardDirectorRepository _repository;

        public UpdateLegalBoardDirectorCommandHandler(ILegalBoardDirectorRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(UpdateLegalBoardDirectorCommand request, CancellationToken cancellationToken)
        {
            var data = UtilityBusinessLink.PassDataOriginDestiny(request.LegalBoardDirector, new LegalBoardDirector());
            await _repository.UpdatelegalBoardDirector(data);
            return true;
        }
    }
}
