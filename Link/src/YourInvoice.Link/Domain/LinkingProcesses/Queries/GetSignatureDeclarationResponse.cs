///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetSignatureDeclarationResponse
    {
        public Guid Id { get; set; }
        public Guid? Id_GeneralInformation { get; set; }
        public bool? GeneralStatement { get; set; }
        public bool? VisitAuthorization { get; set; }
        public bool? SourceFundsDeclaration { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }
    }
}