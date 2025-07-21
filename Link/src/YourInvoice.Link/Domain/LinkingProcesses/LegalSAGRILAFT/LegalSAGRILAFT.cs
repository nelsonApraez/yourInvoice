///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT
{
    public class LegalSAGRILAFT : AggregateRoot
    {
        public LegalSAGRILAFT(Guid id, Guid id_LegalGeneralInformation, Guid? questionIdentifier,
            Guid? responseIdentifier, string? responseDetail, Guid? completed, Guid? statusId, DateTime? statusDate)
        {
            Id = id;
            Id_LegalGeneralInformation = id_LegalGeneralInformation;
            QuestionIdentifier = questionIdentifier;
            ResponseIdentifier = responseIdentifier;
            ResponseDetail = responseDetail;
            Completed = completed;
            StatusId = statusId;
            StatusDate = statusDate;
        }

        public Guid Id_LegalGeneralInformation { get; set; }
        public Guid? QuestionIdentifier { get; set; }
        public Guid? ResponseIdentifier { get; set; }
        public string? ResponseDetail { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
    }
}