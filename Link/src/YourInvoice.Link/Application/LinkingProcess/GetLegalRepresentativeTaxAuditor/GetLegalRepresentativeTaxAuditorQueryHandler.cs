///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalRepresentativeTaxAuditor
{
    public class GetLegalRepresentativeTaxAuditorQueryHandler : IRequestHandler<GetLegalRepresentativeTaxAuditorQuery, ErrorOr<LegalRepresentativeTaxAuditor>>
    {
        private readonly ILegalRepresentativeTaxAuditorRepository _repository;

        public GetLegalRepresentativeTaxAuditorQueryHandler(ILegalRepresentativeTaxAuditorRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<LegalRepresentativeTaxAuditor>> Handle(GetLegalRepresentativeTaxAuditorQuery query, CancellationToken cancellationToken)
        {
            var idCurrentUser = query?.Id_LegalGeneralInformation ?? Guid.Empty;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var result = await _repository.GetLegalRepresentativeTaxAuditorAsync(idCurrentUser);
            return result;
        }
    }
}