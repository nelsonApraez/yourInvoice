///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalShareholder;

public record UpdateLegalShareholderCommand(LegalShareholder LegalShareholder) : IRequest<ErrorOr<bool>>;