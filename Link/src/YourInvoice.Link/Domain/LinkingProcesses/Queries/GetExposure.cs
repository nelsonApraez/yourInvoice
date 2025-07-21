///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetExposure
    {
        public Guid Id { get; set; }
        public Guid? Id_GeneralInformation { get; set; }
        public Guid? QuestionIdentifier { get; set; }
        public string QuestionIdentifierDescription { get; set; }
        public Guid? ResponseIdentifier { get; set; }
        public string ResponseIdentifierDescription { get; set; }
        public string? ResponseDetail { get; set; }
    }
}