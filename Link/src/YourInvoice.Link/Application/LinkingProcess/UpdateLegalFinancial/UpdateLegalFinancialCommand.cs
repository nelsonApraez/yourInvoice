namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalFinancial
{
    public record UpdateLegalFinancialCommand(UpdateLegalFinancial UpdateFinancials) : IRequest<ErrorOr<bool>>;
}
