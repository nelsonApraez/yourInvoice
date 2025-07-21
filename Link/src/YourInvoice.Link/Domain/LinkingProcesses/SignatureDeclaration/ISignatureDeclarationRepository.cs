using yourInvoice.Link.Domain.LinkingProcesses.Queries;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration
{
    public interface ISignatureDeclarationRepository
    {
        Task<bool> CreateSignatureDeclarationAsync(SignatureDeclaration signatureDeclaration);
        Task<bool> ExistsSignatureDeclarationByIdAsync(Guid idGeneralInformation);
        Task<GetSignatureDeclarationResponse> GetSignatureDeclarationAsync(Guid idGeneralInformation);
        Task<bool> UpdateSignatureDeclarationAsync(SignatureDeclaration signature);


    }
}