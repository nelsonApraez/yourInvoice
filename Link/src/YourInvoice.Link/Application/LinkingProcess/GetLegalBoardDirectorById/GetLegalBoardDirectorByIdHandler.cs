///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalBoardDirectorById
{
    public class GetLegalBoardDirectorByIdHandler : IRequestHandler<GetLegalBoardDirectorByIdQuery, ErrorOr<List<GetLegalBoardDirectorResponse>>>
    {
        private readonly ILegalBoardDirectorRepository _repository;

        public GetLegalBoardDirectorByIdHandler(ILegalBoardDirectorRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<List<GetLegalBoardDirectorResponse>>> Handle(GetLegalBoardDirectorByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetLegalBoardDirectorById(request.id_LegalGeneralInformation);
            return result;
        }
    }
}
