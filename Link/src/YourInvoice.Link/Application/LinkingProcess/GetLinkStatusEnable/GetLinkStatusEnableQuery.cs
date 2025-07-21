///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLinkStatusEnable;

public record GetLinkStatusEnableQuery(Guid IdUserLink) : IRequest<ErrorOr<GetLinkStatusEnableResponse>>;