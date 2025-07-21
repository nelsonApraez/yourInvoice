///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GetGeneralInformation
{
    public class GetGeneralInformationQueryHandler : IRequestHandler<GetGeneralInformationQuery, ErrorOr<GeneralInformationResponse>>
    {
        private readonly IGeneralInformationRepository _repository;

        public GetGeneralInformationQueryHandler(IGeneralInformationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GeneralInformationResponse>> Handle(GetGeneralInformationQuery query, CancellationToken cancellationToken)
        {
            if (await _repository.GetGeneralInformationIdAsync(query.Id) is not GeneralInformationResponse generalInformationResponse)
            {
                return Error.NotFound(MessageCodes.GeneralInformationNotExist, GetErrorDescription(MessageCodes.GeneralInformationNotExist));
            }

            return
                generalInformationResponse;
        }
    }
}