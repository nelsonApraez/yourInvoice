///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors
{
    public interface ILegalShareholderBoardDirectorRepository
    {
        LegalShareholderBoardDirector AddLegalShareholderBoardDirector(LegalShareholderBoardDirector entity);

        Task<GetLegalShareholderBoardDirectorResponse> GetLegalShareholderBoardDirectorById(Guid id_LegalGeneralInformation);

        Task<bool> UpdateLegalShareholderBoardDirector(LegalShareholderBoardDirector entity);

        Task<bool> ExistsLegalShareholderBoardDirectorById(Guid id_LegalGeneralInformation);
    }
}
