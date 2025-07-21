///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalSignatureDeclaration;

public record GetLegalSignatureDeclarationQuery(Guid Id_LegalGeneralInformation) : IRequest<ErrorOr<IEnumerable<GetLegalSignatureDeclarationResponse>>>;