///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalShareholder;

public record CreateLegalShareholderCommand(Guid id_LegalGeneralInformation, string fullNameCompanyName, Guid documentTypeId, string documentNumber, string phoneNumber, Guid? completed) : IRequest<ErrorOr<bool>>;
