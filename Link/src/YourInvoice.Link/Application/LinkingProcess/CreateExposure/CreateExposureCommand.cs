///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateExposure;

public record CreateExposureCommand(IEnumerable<Exposure> CreateExposures) : IRequest<ErrorOr<bool>>;