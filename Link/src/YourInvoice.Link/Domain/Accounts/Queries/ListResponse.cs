///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.Accounts.Queries
{
    public class ListResponse
    {

        public Guid Id { get; set; }

        public int OrderRegister { get; set; }

        public string NameOrder { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public Guid UserTypeId { get; set; }
        public string UserType { get; set; }
        public Guid? PersonTypeId { get; set; }
        public string PersonType { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public string DocumentType { get; set; }

        public string DocumentNumber { get; set; }

        public int? Time { get; set; }
        public Guid? StatusId { get; set; }
        public string Status { get; set; }

        public DateTime? StatusDate { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}