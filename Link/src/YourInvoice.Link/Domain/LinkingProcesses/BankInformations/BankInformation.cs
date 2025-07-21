///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.BankInformations
{
    public class BankInformation : AggregateRoot
    {
        public BankInformation()
        {
        }

        public BankInformation(Guid id, Guid id_GeneralInformation, Guid? bankReference, string? phoneNumber,
                     string? bankProduct, Guid? departmentState, Guid? city, Guid? completed, DateTime? createdOn, Guid? statusId, DateTime? statusDate)
        {
            Id = id;
            Id_GeneralInformation = id_GeneralInformation;
            BankReference = bankReference;
            PhoneNumber = phoneNumber;
            BankProduct = bankProduct;
            DepartmentState = departmentState;
            City = city;
            Completed = completed;
            CreatedOn = createdOn;
            StatusId = statusId;
            StatusDate = statusDate;
        }

        public Guid Id_GeneralInformation { get; set; }
        public Guid? BankReference { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BankProduct { get; set; }
        public Guid? DepartmentState { get; set; }
        public Guid? City { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
    }
}