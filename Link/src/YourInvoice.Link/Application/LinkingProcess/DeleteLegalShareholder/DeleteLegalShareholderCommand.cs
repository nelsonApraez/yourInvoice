///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.DeleteLegalShareholder;

public record DeleteLegalShareholderCommand(Guid Id, Guid Id_LegalGeneralInformation) : IRequest<ErrorOr<bool>>;