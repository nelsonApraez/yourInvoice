///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalRepresentativeTaxAuditor;

public record CreateLegalRepresentativeTaxAuditorCommand(LegalRepresentativeTax LegalRepresentative) : IRequest<ErrorOr<bool>>;