///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors
{
    public class LegalShareholderBoardDirector : AggregateRoot
    {
        public LegalShareholderBoardDirector() { }

        public LegalShareholderBoardDirector(Guid id, Guid id_LegalGeneralInformation, bool isSoleProprietorship, Guid? completed, DateTime? createOn, Guid? createBy, 
            DateTime? modifiedOn, Guid? modifiedBy, Guid? statusId, DateTime? statusDate, bool? status)
        {
            Id = id;
            Id_LegalGeneralInformation = id_LegalGeneralInformation;
            IsSoleProprietorship = isSoleProprietorship;
            Completed = completed;
            StatusId = statusId;
            StatusDate = statusDate;
            Status = status;
            CreatedOn = createOn;
            CreatedBy = createBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
        }

        public Guid? Id_LegalGeneralInformation { get; set; }

        public bool? IsSoleProprietorship { get; set; }

        public Guid? Completed { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }

    }
}
