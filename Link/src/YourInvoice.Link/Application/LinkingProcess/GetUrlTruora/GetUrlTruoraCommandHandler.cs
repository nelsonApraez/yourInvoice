using yourInvoice.Common.Integration.Truora;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Common.Business.CatalogModule;

namespace yourInvoice.Link.Application.LinkingProcess.GetUrlTruora
{
    public class GetUrlTruoraCommandHandler : IRequestHandler<GetUrlTruoraCommand, ErrorOr<Dictionary<string, string>>>
    {
        private readonly ITruora _truora;
        private readonly ILinkStatusRepository _linkStatusRepository;

        public GetUrlTruoraCommandHandler(ITruora truora, ILinkStatusRepository linkStatusRepository)
        {
            _truora = truora ?? throw new ArgumentNullException(nameof(truora));
            _linkStatusRepository = linkStatusRepository ?? throw new ArgumentNullException(nameof(linkStatusRepository));
        }

        public async Task<ErrorOr<Dictionary<string, string>>> Handle(GetUrlTruoraCommand command, CancellationToken cancellationToken) 
        {
            LinkStatus statusId = await _linkStatusRepository.GetLinkStatusAsync(command.generalInformationId);

            if (statusId.StatusLinkId == CatalogCodeLink_LinkStatus.PendingApproval)
                return Error.Validation(MessageCodes.PendigApproval, GetErrorDescription(MessageCodes.PendigApproval));

            Dictionary<string, string> result = await _truora.CreateApiKeyAsync(command.generalInformationId);

            return result;
        }
    }
}
