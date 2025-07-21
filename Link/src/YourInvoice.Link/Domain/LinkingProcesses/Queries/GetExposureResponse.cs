///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetExposureResponse
    {
        public Guid? Completed { get; set; }
        public string CompletedDescription { get; set; }
        public string CompletedName { get; set; }
        public string? DeclarationOriginFunds { get; set; }
        public IEnumerable<GetExposure> ExposureAnswers { get; set; }
    }
}