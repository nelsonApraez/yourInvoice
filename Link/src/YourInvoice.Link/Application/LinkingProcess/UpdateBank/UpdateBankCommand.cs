
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.UpdateBank
{
    public record UpdateBankCommand(UpdateBank UpdateBanks) : IRequest<ErrorOr<bool>>;
}