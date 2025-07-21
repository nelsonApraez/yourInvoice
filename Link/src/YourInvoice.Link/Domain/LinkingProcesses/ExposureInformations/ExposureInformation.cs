///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations
{
    public class ExposureInformation : AggregateRoot
    {
        public ExposureInformation(Guid id, Guid id_GeneralInformation, Guid? questionIdentifier,
            Guid? responseIdentifier, string? responseDetail, Guid? completed, Guid? statusId, DateTime? statusDate, string? declarationOriginFunds)
        {
            Id = id;
            Id_GeneralInformation = id_GeneralInformation;
            QuestionIdentifier = questionIdentifier;
            ResponseIdentifier = responseIdentifier;
            ResponseDetail = responseDetail;
            Completed = completed;
            StatusId = statusId;
            StatusDate = statusDate;
            DeclarationOriginFunds = declarationOriginFunds;
        }

        public Guid Id_GeneralInformation { get; set; }
        public Guid? QuestionIdentifier { get; set; }
        public Guid? ResponseIdentifier { get; set; }
        public string? ResponseDetail { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
        public string? DeclarationOriginFunds { get; set; }
    }
}