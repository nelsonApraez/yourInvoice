///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalShareholderById;

public record GetLegalShareholderByIdQuery(Guid id_LegalGeneralInformation) : IRequest<ErrorOr<List<GetLegalShareholderResponse>>>;

