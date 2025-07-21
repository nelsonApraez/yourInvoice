///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.UpdateFinancial
{
    public record UpdateFinancialCommand(UpdateFinancial UpdateFinancials) : IRequest<ErrorOr<bool>>;
}