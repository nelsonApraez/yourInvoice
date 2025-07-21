///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration
{
    public class SignatureDeclaration : AggregateRoot
    {
        public SignatureDeclaration()
        { }

        public SignatureDeclaration(Guid id, Guid id_GeneralInformation, bool? generalStatement,
           bool? visitAuthorization, bool? sourceFundsDeclaration, Guid? completed, Guid? statusId, DateTime? statusDate, bool? status, DateTime? modifiedOn, Guid? modifiedBy, DateTime? createdOn, Guid? createdBy)
        {
            Id = id;
            Id_GeneralInformation = id_GeneralInformation;
            GeneralStatement = generalStatement;
            VisitAuthorization = visitAuthorization;
            SourceFundsDeclaration = sourceFundsDeclaration;
            Completed = completed;
            StatusId = statusId;
            StatusDate = statusDate;
            Status = status;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }

        public Guid Id_GeneralInformation { get; set; }
        public bool? GeneralStatement { get; set; }
        public bool? VisitAuthorization { get; set; }
        public bool? SourceFundsDeclaration { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }
        public bool? Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}