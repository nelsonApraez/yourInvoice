///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetWorking;

public record GetWorkingQuery(Guid Id_GeneralInformation) : IRequest<ErrorOr<GetWorkingResponse>>;