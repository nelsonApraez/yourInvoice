///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.LinkingProcess.UpdateSignatureDeclaration
{
    public record UpdateSignatureDeclarationCommand(UpdateSignatureDeclaration UpdateSignatureDeclaration) : IRequest<ErrorOr<bool>>;
}