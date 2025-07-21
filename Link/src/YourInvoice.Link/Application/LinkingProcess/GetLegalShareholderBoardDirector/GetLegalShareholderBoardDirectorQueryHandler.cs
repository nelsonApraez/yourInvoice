using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalShareholderBoardDirector
{
    public class GetLegalShareholderBoardDirectorQueryHandler : IRequestHandler<GetLegalShareholderBoardDirectorQuery, ErrorOr<GetLegalShareholderBoardDirectorResponse>>
    {
        private readonly ILegalShareholderBoardDirectorRepository _repository;

        public GetLegalShareholderBoardDirectorQueryHandler(ILegalShareholderBoardDirectorRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        public async Task<ErrorOr<GetLegalShareholderBoardDirectorResponse>> Handle(GetLegalShareholderBoardDirectorQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetLegalShareholderBoardDirectorById(request.id_LegalGeneralInformation);
            return result;
        }
    }
}
