///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences
{
    public class PersonalReferences : AggregateRoot
    {
        public PersonalReferences() { }

        public PersonalReferences(Guid id, Guid? id_GeneralInformation, string? namePersonalReference, string? phoneNumber, string? nameBussines,
                Guid? departmentState, Guid? city, Guid? completed, Guid? statusId, DateTime? statusDate, bool? status, DateTime? createOn, Guid? createBy, DateTime? modifiedOn, Guid? modifiedBy)
        {
            Id = id;
            this.Id_GeneralInformation = id_GeneralInformation;
            this.NamePersonalReference = namePersonalReference;
            this.PhoneNumber = phoneNumber;
            this.NameBussines = nameBussines;
            this.DepartmentState = departmentState;
            this.City = city;
            this.Completed = completed;
            this.StatusId = statusId;
            this.StatusDate = statusDate;
            Status = status;
            CreatedOn = createOn;
            CreatedBy = createBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;            
        }

        public Guid? Id_GeneralInformation { get; set; }

        public string? NamePersonalReference { get; set; }

        public string? PhoneNumber { get; set; }

        public string? NameBussines { get; set; }

        public Guid? DepartmentState { get; set; }

        public Guid? City { get; set; }

        public Guid? Completed { get; set; }

        public bool? Status { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}
