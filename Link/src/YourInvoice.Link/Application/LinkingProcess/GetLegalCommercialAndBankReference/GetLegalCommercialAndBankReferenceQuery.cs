using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalCommercialAndBankReference
{
    public record GetLegalCommercialAndBankReferenceQuery(Guid idLegalGeneralInformation) : IRequest<ErrorOr<LegalCommercialAndBankReferenceResponse>>;
}
