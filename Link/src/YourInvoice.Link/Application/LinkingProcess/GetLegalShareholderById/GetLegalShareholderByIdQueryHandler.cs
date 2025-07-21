///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalShareholderById
{
    public class GetLegalShareholderByIdQueryHandler : IRequestHandler<GetLegalShareholderByIdQuery, ErrorOr<List<GetLegalShareholderResponse>>>
    {
        private readonly ILegalShareholderRepository _repository;

        public GetLegalShareholderByIdQueryHandler(ILegalShareholderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<List<GetLegalShareholderResponse>>> Handle(GetLegalShareholderByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetLegalShareholdersById(request.id_LegalGeneralInformation);
            return result;
        }
    }
}
