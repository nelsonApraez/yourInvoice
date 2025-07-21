///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class ListLinkingProccessResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserTypeId { get; set; }
        public string UserType { get; set; }
        public Guid? PersonTypeId { get; set; }
        public string PersonType { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public int? CompletionPercentage { get; set; }
        public Guid? StatusId { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}