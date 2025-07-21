///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.DeleteLegalBoardDirector;

public record DeleteLegalBoardDirectorCommand(Guid Id, Guid Id_LegalGeneralInformation) : IRequest<ErrorOr<bool>>;