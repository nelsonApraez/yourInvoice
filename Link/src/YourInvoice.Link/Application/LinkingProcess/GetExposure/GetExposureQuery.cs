///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetExposure;

public record GetExposureQuery(Guid idGeneralInformation) : IRequest<ErrorOr<GetExposureResponse>>;