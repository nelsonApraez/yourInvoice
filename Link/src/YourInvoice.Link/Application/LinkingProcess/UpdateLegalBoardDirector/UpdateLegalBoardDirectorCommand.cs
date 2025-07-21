///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalBoardDirector;

public record UpdateLegalBoardDirectorCommand(LegalBoardDirector LegalBoardDirector) : IRequest<ErrorOr<bool>>;