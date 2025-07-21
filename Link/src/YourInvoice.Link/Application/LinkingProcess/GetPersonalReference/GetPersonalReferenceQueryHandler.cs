///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetPersonalReference
{
    public class GetPersonalReferenceQueryHandler : IRequestHandler<GetPersonalReferenceQuery, ErrorOr<GetReferenceResponse>>
    {
        private readonly IPersonalReferenceRepository _repository;

        public GetPersonalReferenceQueryHandler(IPersonalReferenceRepository repository)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GetReferenceResponse>> Handle(GetPersonalReferenceQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetPersonalReferenceAsync(request.idGeneralInformation);
            return result;
        }
    }
}
