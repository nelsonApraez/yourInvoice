///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetPersonalReference;

public record GetPersonalReferenceQuery(Guid idGeneralInformation) : IRequest<ErrorOr<GetReferenceResponse>>;