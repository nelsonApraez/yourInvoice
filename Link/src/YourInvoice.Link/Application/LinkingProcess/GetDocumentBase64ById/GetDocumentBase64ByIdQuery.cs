///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.GetDocumentBase64ById;

public record GetDocumentBase64ByIdQuery(Guid idDocument) : IRequest<ErrorOr<string[]>>;