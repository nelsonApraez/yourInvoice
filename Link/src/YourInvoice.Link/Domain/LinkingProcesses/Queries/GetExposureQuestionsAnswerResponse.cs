///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetExposureQuestionsAnswerResponse
    {
        public Guid IdQuestion { get; set; }
        public string DecriptionQuestion { get; set; }
        public bool Detalle { get; set; }
        public IEnumerable<GetExposureAnswer> Answers { get; set; }
    }
}