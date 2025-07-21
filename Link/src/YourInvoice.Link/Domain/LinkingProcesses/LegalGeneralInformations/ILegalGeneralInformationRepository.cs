
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations
{
    public interface ILegalGeneralInformationRepository
    {
        Task<LegalGeneralInformation?> GetByEmailAsync(string email);
        LegalGeneralInformation Add(LegalGeneralInformation generalInformation);
        Task<bool> CreateLegalGeneralInformationAsync(LegalGeneralInformation legalGeneralInformation);

        Task<bool> ExistsAccountLegalAsync(Guid accountId, Guid personTypeId);

        Task<bool> ExistseLegalGeneralInformationAsync(Guid accountId);

        Task<GetLegalGeneralInformationResponse> GetLegalGeneralInformationAsync(Guid accountId);

        Task<Guid> GetStatusIdLegalGeneralInformationAsync(Guid accountId);

        Task<bool> UpdateLegalGeneralInformationAsync(LegalGeneralInformation legalGeneralInformation);

        Task<bool> UpdateStatusAsync(Guid id, Guid statusId);
    }
}