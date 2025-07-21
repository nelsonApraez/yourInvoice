///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLinkStatusEnable
{
    public class GetLinkStatusEnableQueryHandler : IRequestHandler<GetLinkStatusEnableQuery, ErrorOr<GetLinkStatusEnableResponse>>
    {
        private readonly ILinkStatusRepository _repository;

        public GetLinkStatusEnableQueryHandler(ILinkStatusRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GetLinkStatusEnableResponse>> Handle(GetLinkStatusEnableQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetLinkStatusDisabledAsync(query.IdUserLink);

            result.DisabledField = (CatalogCodeLink_LinkStatus.PendingApproval == result.LinkStatusId
                                    || CatalogCodeLink_LinkStatus.Linked == result.LinkStatusId
                                    || CatalogCodeLink_LinkStatus.Rejected == result.LinkStatusId);

            return result;
        }
    }
}