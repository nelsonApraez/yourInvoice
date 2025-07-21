///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalRepresentativeTaxAuditor;
public record GetLegalRepresentativeTaxAuditorQuery(Guid Id_LegalGeneralInformation) : IRequest<ErrorOr<LegalRepresentativeTaxAuditor>>;