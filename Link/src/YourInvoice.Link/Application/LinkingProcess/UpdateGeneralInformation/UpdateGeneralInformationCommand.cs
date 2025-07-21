
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.LinkingProcess.UpdateGeneralInformation;

public record UpdateGeneralInformationCommand(GeneralInformation generalInformation) : IRequest<ErrorOr<bool>>;