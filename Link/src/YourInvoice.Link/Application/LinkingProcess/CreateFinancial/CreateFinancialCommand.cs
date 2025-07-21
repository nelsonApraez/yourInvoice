namespace yourInvoice.Link.Application.LinkingProcess.CreateFinancial;

public record CreateFinancialCommand(Financial CreateFinancials) : IRequest<ErrorOr<bool>>;