///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors
{
    public class LegalBoardDirector : AggregateRoot
    {
        public LegalBoardDirector() { }

        public LegalBoardDirector(Guid id, Guid id_LegalGeneralInformation, string fullNameCompanyName, Guid documentTypeId, string documentNumber, string phoneNumber,
            Guid? completed, Guid? statusId, DateTime? statusDate, bool? status, DateTime? createOn, Guid? createBy, DateTime? modifiedOn, Guid? modifiedBy)
        {
            Id = id;
            Id_LegalGeneralInformation = id_LegalGeneralInformation;
            FullNameCompanyName = fullNameCompanyName;
            DocumentTypeId = documentTypeId;
            DocumentNumber = documentNumber;
            PhoneNumber = phoneNumber;
            Completed = completed;
            StatusId = statusId;
            StatusDate = statusDate;
            Status = status;
            Status = status;
            CreatedOn = createOn;
            CreatedBy = createBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
        }

        public Guid? Id_LegalGeneralInformation { get; set; }

        public string? FullNameCompanyName { get; set; }

        public Guid? DocumentTypeId { get; set; }

        public string? DocumentNumber { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? Completed { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}