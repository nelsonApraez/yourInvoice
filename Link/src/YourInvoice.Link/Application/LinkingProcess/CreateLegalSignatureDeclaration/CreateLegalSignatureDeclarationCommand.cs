///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalSignatureDeclaration;

public record CreateLegalSignatureDeclarationCommand(LegalSignatureDeclarationCommand LegalSignatureDeclaration) : IRequest<ErrorOr<bool>>;