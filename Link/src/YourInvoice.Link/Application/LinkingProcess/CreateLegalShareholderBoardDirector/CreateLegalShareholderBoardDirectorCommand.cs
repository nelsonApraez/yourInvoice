///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalShareholderBoardDirector;

public record CreateLegalShareholderBoardDirectorCommand(Guid id_LegalGeneralInformation, bool isSoleProprietorship, Guid? completed) : IRequest<ErrorOr<bool>>;
