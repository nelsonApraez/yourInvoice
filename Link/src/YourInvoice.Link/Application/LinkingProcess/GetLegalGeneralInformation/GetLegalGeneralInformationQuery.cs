///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalGeneralInformation;

public record GetLegalGeneralInformationQuery(Guid Id) : IRequest<ErrorOr<GetLegalGeneralInformationResponse>>;