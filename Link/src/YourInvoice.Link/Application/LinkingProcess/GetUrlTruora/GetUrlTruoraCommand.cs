namespace yourInvoice.Link.Application.LinkingProcess.GetUrlTruora
{
    public record GetUrlTruoraCommand(Guid generalInformationId) : IRequest<ErrorOr<Dictionary<string, string>>>;
}
