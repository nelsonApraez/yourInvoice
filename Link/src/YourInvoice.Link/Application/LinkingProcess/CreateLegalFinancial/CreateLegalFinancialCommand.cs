namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalFinancial
{

    public record CreateLegalFinancialCommand(LegalFinancial CreateLegalFinancials) : IRequest<ErrorOr<bool>>;
}
