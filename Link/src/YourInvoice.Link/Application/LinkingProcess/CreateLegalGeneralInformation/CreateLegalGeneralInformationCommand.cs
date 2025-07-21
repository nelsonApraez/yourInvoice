///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalGeneralInformation;

public record CreateLegalGeneralInformationCommand(LegalGeneral LegalGeneral) : IRequest<ErrorOr<bool>>;