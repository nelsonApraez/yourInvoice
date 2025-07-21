///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetStatusForm;

public record GetStatusFormQuery(Guid Id_GeneralInformation) : IRequest<ErrorOr<GetStatusFormResponse>>;