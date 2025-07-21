///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetStatusFormLegal;

public record GetStatusFormLegalQuery(Guid Id_LegalGeneralInformation) : IRequest<ErrorOr<GetStatusFormLegalResponse>>;