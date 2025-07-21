namespace yourInvoice.Link.Application.LinkingProcess.SignDocs
{
    public record class SignDocsLinkCommand(Guid generalInformationId) : IRequest<ErrorOr<SignDocsResponse>>;
}
