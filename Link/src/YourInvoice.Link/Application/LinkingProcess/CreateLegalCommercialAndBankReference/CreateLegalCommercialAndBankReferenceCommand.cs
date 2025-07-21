
namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalCommercialAndBankReference
{
    public record CreateLegalCommercialAndBankReferenceCommand(References References) : IRequest<ErrorOr<bool>>;
}
