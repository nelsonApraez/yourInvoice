///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations
{
    public class LegalSignatureDeclaration : AggregateRoot
    {
        public LegalSignatureDeclaration()
        { }

        public LegalSignatureDeclaration(Guid id, Guid id_LegalGeneralInformation, bool? commitmentAcceptRiskManagement, bool? responsivilityForInformation,
                                         bool? visitAuthorization, bool? statements, Guid? completed, Guid? statusId, DateTime? statusDate, bool? status,
                                         DateTime? modifiedOn, Guid? modifiedBy, DateTime? createdOn, Guid? createdBy)
        {
            Id = id;
            Id_LegalGeneralInformation = id_LegalGeneralInformation;
            CommitmentAcceptRiskManagement = commitmentAcceptRiskManagement;
            ResponsivilityForInformation = responsivilityForInformation;
            VisitAuthorization = visitAuthorization;
            Statements = statements;
            Completed = completed;
            StatusId = statusId;
            StatusDate = statusDate;
            Status = status;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }

        public Guid Id_LegalGeneralInformation { get; private set; }
        public bool? CommitmentAcceptRiskManagement { get; private set; }
        public bool? ResponsivilityForInformation { get; private set; }
        public bool? Statements { get; private set; }
        public bool? VisitAuthorization { get; private set; }
        public Guid? Completed { get; private set; }
        public Guid? StatusId { get;  set; }
        public DateTime? StatusDate { get; private set; }
    }
}