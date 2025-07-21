using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference
{
    public interface ILegalCommercialAndBankReferenceRepository
    {
        Task<bool> ExistsLegalCommercialAndBankReferenceAsync(Guid idLegalGeneralInformation);

        public Task<bool> CreateLegalCommercialAndBankReferenceAsync(LegalCommercialAndBankReference reference);

        public Task<bool> UpdateLegalCommercialAndBankReferenceAsync(LegalCommercialAndBankReference reference);

        public Task<LegalCommercialAndBankReferenceResponse> GetLegalCommercialAndBankReferenceAsync(Guid Id);
    }
}
