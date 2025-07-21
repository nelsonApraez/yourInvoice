///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class UserInfo : ModelBase
    {
        public int IntegrationId { get; set; }
        public string Aadid { get; set; }
        public string Name { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentExpedition { get; set; }
        public string Job { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public Guid UserTypeId { get; set; }
        public Guid RoleId { get; set; }
        public string Company { get; set; }
        public string CompanyNit { get; set; }
        public string CompanyNitDv { get; set; }
        public string CompanyCommercialRegistrationNumber { get; set; } //numero de matricula mercantil
        public string CompanyCommercialRegistrationCity { get; set; } //ciudad de matricula mercantil
        public string CompanyChamberOfCommerceCity { get; set; } //ciudad de camara de comercio
    }
}