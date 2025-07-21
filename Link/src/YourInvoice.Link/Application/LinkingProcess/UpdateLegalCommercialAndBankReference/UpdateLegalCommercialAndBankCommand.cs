
namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalCommercialAndBankReference
{
    public record UpdateLegalCommercialAndBankCommand(UpdateCommercialAndBank UpdateCommercialAndBank) : IRequest<ErrorOr<bool>>;
}
