
namespace yourInvoice.Link.Application.LinkingProcess.ValidateProcessTruora
{
    public record class ValidateProcessTruoraCommand(string processId, Guid generalInformationId) : IRequest<ErrorOr<bool>>;
}
