namespace yourInvoice.Link.Application.LinkingProcess.SignSuccessDocs
{
    public record class SignSuccessDocsLinkCommand(Guid generalInformationId) : IRequest<ErrorOr<bool>>;
}
