///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.SellerMoneyTransfers;

namespace yourInvoice.Offer.Domain.Users
{
    public sealed class User : AggregateRoot
    {
        public User()
        {
        }

        public User(Guid id
                    , int integrationId
                    , string aadid
                    , string name
                    , Guid documentTypeId
                    , string documentNumber
                    , string documentExpedition
                    , string job
                    , string address
                    , string phone
                    , string email
                    , string city
                    , Guid userTypeId
                    , Guid roleId
                    , string company
                    , string companyNit
                    , string companyNitDv
                    , string companyCommercialRegistrationNumber
                    , string companyCommercialRegistrationCity
                    , string companyChamberOfCommerceCity
                    , bool status
                    , DateTime createdOn
                    , Guid createdBy
                    , DateTime modifiedOn
                    , Guid modifiedBy)
        {
            Id = id;
            IntegrationId = integrationId;
            Aadid = aadid;
            Name = name;
            DocumentTypeId = documentTypeId;
            DocumentNumber = documentNumber;
            DocumentExpedition = documentExpedition;
            Job = job;
            Address = address;
            Phone = phone;
            Email = email;
            City = city;
            UserTypeId = userTypeId;
            RoleId = roleId;
            Company = company;
            CompanyNit = companyNit;
            CompanyNitDv = companyNitDv;
            CompanyCommercialRegistrationNumber = companyCommercialRegistrationNumber;
            CompanyCommercialRegistrationCity = companyCommercialRegistrationCity;
            CompanyChamberOfCommerceCity = companyChamberOfCommerceCity;
            Status = status;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
        }

        public int IntegrationId { get; private set; }
        public string Aadid { get; private set; }
        public string Name { get; private set; }
        public Guid DocumentTypeId { get; private set; }
        public string DocumentNumber { get; private set; }
        public string DocumentExpedition { get; private set; }
        public string Job { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; set; }
        public string City { get; private set; }
        public Guid UserTypeId { get; private set; }
        public Guid RoleId { get; private set; }
        public string Company { get; private set; }
        public string CompanyNit { get; private set; }
        public string CompanyNitDv { get; private set; }
        public string CompanyCommercialRegistrationNumber { get; private set; } //numero de matricula mercantil
        public string CompanyCommercialRegistrationCity { get; private set; } //ciudad de matricula mercantil
        public string CompanyChamberOfCommerceCity { get; private set; } //ciudad de camara de comercio

        public ICollection<Offer> Offers { get; private set; } = new List<Offer>();

        public ICollection<SellerMoneyTransfer> SellerMoneyTransfers { get; set; } = new List<SellerMoneyTransfer>();
    }
}