///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders
{
    public interface ILegalShareholderRepository
    {
        LegalShareholder AddLegalShareholder(LegalShareholder legalShareholder);

        Task<List<GetLegalShareholderResponse>> GetLegalShareholdersById(Guid id_LegalGeneralInformation);

        Task<bool> UpdateLegalShareholder(LegalShareholder legalShareholder);

        Task<bool> ExistsLegalShareholderById(Guid id, Guid id_LegalGeneralInformation);

        Task<bool> DeleteLegalShareholder(Guid id, Guid id_LegalGeneralInformation);

        Task<bool> ExistsLegalShareholder(Guid id_LegalGeneralInformation);
    }
}
