///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.UpdateExposure
{
    public record UpdateExposureCommand(IEnumerable<UpdateExposure> UpdateExposures) : IRequest<ErrorOr<bool>>;
}