///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.GenerateDocuments;
public record class GenerateDocumentsCommand(Guid idGeneralInformation) : IRequest<ErrorOr<bool>>;