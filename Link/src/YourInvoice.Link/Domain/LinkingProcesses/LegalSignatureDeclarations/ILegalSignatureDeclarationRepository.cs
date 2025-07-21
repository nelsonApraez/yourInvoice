///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations
{
    public interface ILegalSignatureDeclarationRepository
    {
        Task<bool> CreateLegalSignatureDeclarationAsync(LegalSignatureDeclaration legalSignatureDeclaration);

        Task<bool> ExistsLegalSignatureDeclarationAsync(Guid idLegalGeneralInformation);

        Task<IEnumerable<string>> GeParagraphAsync(string catalogName);

        Task<GetAccounLegalGeneralResponse> GetAccounLegalGeneralAsync(Guid idLegalGeneralInformation);

        Task<LegalSignatureDeclaration> GetLegalSignatureDeclarationAsync(Guid idLegalGeneralInformation);

        Task<bool> UpdateLegalSignatureDeclarationAsync(LegalSignatureDeclaration legalSignatureDeclaration);
    }
}