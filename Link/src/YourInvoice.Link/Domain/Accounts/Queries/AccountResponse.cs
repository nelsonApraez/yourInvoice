///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.Accounts.Queries
{
    public class AccountResponse : Account
    {
        public string PersonType { get; set; }
        public string CustomerType { get; set; }
        public string DocumentType { get; set; }
        public string MobileCountry { get; set; }
        public string PhoneCountry { get; set; }
        public string ContactBy { get; set; }
        public string Status { get; set; }
        public Guid RoleId { get; set; }
        public string Date { get; set; }
    }
}