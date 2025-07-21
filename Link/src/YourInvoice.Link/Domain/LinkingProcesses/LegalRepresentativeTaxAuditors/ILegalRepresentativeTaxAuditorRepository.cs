///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors
{
    public interface ILegalRepresentativeTaxAuditorRepository
    {
        Task<bool> CreateLegalRepresentativeTaxAuditorRepositoryAsync(LegalRepresentativeTaxAuditor legalRepresentativeTax);

        Task<bool> ExistseLegalRepresentativeTaxAuditorRepositoryAsync(Guid id_legalRepresentativeTax);

        Task<LegalRepresentativeTaxAuditor> GetLegalRepresentativeTaxAuditorAsync(Guid id_LegalGeneralInformation);
        Task<bool> UpdateAccountAsync(LegalRepresentativeTaxAuditor legalRepresentativeTax);
        Task<bool> UpdateLegalRepresentativeTaxAuditorAsync(LegalRepresentativeTaxAuditor legalRepresentativeTax);
    }
}