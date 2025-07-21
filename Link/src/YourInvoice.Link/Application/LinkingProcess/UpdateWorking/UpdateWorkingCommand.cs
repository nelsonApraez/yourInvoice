///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.UpdateWorking;

public record UpdateWorkingCommand(UpdateWorking updateWorking) : IRequest<ErrorOr<bool>>;