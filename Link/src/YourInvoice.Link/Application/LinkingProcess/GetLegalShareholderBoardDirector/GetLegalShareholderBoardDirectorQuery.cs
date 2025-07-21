///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalShareholderBoardDirector;

public record GetLegalShareholderBoardDirectorQuery(Guid id_LegalGeneralInformation) : IRequest<ErrorOr<GetLegalShareholderBoardDirectorResponse>>;