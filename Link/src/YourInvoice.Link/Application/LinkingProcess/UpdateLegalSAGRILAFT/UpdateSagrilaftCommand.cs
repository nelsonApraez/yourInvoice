///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalSAGRILAFT
{
    public record UpdateSagrilaftCommand(IEnumerable<UpdateSagrilaft> UpdateSagrilaft) : IRequest<ErrorOr<bool>>;
}