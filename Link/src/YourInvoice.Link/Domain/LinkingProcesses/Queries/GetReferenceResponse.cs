///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetReferenceResponse
    {
        public Guid? Id { get; set; }

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
