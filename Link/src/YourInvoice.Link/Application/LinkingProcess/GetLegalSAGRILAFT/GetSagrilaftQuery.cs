
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFT;

public record GetSagrilaftQuery(Guid idLegalGeneralInformation) : IRequest<ErrorOr<GetSagrilaftResponse>>;