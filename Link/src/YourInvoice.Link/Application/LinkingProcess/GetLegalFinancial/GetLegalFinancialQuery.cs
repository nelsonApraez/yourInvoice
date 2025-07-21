using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalFinancial
{
    public record GetLegalFinancialQuery(Guid idLegalGeneralInformation) : IRequest<ErrorOr<GetLegalFinancialResponse>>;
}
