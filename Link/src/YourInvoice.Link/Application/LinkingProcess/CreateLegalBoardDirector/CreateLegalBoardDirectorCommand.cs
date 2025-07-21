///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalBoardDirector;

public record CreateLegalBoardDirectorCommand(Guid id_LegalGeneralInformation, string fullNameCompanyName, Guid documentTypeId, string documentNumber, string phoneNumber, Guid? completed) : IRequest<ErrorOr<bool>>;