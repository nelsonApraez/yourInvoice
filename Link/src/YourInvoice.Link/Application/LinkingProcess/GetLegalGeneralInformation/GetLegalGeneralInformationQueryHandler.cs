///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalGeneralInformation
{
    public class GetLegalGeneralInformationQueryHandler : IRequestHandler<GetLegalGeneralInformationQuery, ErrorOr<GetLegalGeneralInformationResponse>>
    {
        private readonly ILegalGeneralInformationRepository _repository;

        public GetLegalGeneralInformationQueryHandler(ILegalGeneralInformationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GetLegalGeneralInformationResponse>> Handle(GetLegalGeneralInformationQuery query, CancellationToken cancellationToken)
        {
            var idCurrentUser = query?.Id ?? Guid.Empty;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var result = await _repository.GetLegalGeneralInformationAsync(idCurrentUser);
            return result;
        }
    }
}