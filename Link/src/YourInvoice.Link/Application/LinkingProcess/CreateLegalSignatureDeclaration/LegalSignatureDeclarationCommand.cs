///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalSignatureDeclaration
{
    public class LegalSignatureDeclarationCommand
    {
        public Guid Id_LegalGeneralInformation { get; set; }
        public bool? CommitmentAcceptRiskManagement { get; set; }
        public bool? ResponsivilityForInformation { get; set; }
        public bool? Statements { get; set; }
        public bool? VisitAuthorization { get; set; }
        public Guid? Completed { get; set; }
    }
}