
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetSignatureDeclaration;

public record GetSignatureDeclarationQuery(Guid idGeneralInformation) : IRequest<ErrorOr<GetSignatureDeclarationResponse>>;
