///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateWorking;

public record CreateWorkingCommand(Working working) : IRequest<ErrorOr<bool>>;