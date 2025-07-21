///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateExposure
{
    public class Exposure
    {
        public Guid Id_GeneralInformation { get; set; }
        public Guid? QuestionIdentifier { get; set; }
        public Guid? ResponseIdentifier { get; set; }
        public string? ResponseDetail { get; set; }
        public Guid? Completed { get; set; }
        public string? DeclarationOriginFunds { get; set; }
    }
}