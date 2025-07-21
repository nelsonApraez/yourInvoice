///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.Accounts.Queries
{
    public class ValidityResponse
    {
        public int Process { get; set; }
        public Guid Id { get; set; }
        public string SocialReason { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Guid RoleId { get; set; }
        public string RolName { get; set; }
        public string Email { get; set; }
        public bool IsLegal { get; set; }
        public Guid IdUser { get; set; }
    }
}