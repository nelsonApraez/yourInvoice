///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors
{
    public interface ILegalBoardDirectorRepository
    {
        LegalBoardDirector AddLegalBoardDirector(LegalBoardDirector entity);

        Task<List<GetLegalBoardDirectorResponse>> GetLegalBoardDirectorById(Guid id_LegalGeneralInformation);

        Task<bool> UpdatelegalBoardDirector(LegalBoardDirector legalBoardDirector);

        Task<bool> ExistsLegalBoardDirectorById(Guid id, Guid id_LegalGeneralInformation);

        Task<bool> DeleteLegalBoardDirector(Guid id, Guid id_LegalGeneralInformation);

        Task<bool> ExistsLegalBoardDirector(Guid id_LegalGeneralInformation);
    }
}
