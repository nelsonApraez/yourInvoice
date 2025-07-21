using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations
{
    public interface ILegalFinancialInformationRepository
    {
        Task<bool> ExistsLegalFinancialAsync(Guid idLegalGeneralInformation);

        public Task<bool> CreateLegalFinancialAsync(LegalFinancialInformation financial);

        public Task<bool> UpdateLegalFinancialAsync(LegalFinancialInformation financial);

        public Task<GetLegalFinancialResponse> GetLegalFinancialInformationAsync(Guid Id);
    }
}
