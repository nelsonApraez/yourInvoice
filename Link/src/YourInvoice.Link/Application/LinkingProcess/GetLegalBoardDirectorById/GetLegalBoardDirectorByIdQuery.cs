///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalBoardDirectorById;

public record GetLegalBoardDirectorByIdQuery(Guid id_LegalGeneralInformation) : IRequest<ErrorOr<List<GetLegalBoardDirectorResponse>>>;
