///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.LinkingProcess.SendRequestUpdateDocuments;

public record SendRequestUpdateDocumentCommand(Guid accountId, RequestUpdateDocuments request) : IRequest<ErrorOr<bool>>;