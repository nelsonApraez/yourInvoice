///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetDocumentsByRelatedId;

public record GetDocumentsByRelatedIdQuery(Guid relatedId) : IRequest<ErrorOr<List<GetDocumentResponse>>>;
