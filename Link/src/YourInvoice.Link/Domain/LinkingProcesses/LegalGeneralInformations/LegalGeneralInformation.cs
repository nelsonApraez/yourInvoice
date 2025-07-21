///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations
{
    public class LegalGeneralInformation : AggregateRoot
    {
        public LegalGeneralInformation()
        {
        }

        public LegalGeneralInformation(Guid id, string? nit, string? checkDigit, string? companyName, Guid? companyTypeId, Guid? societyTypeId,
              string? societyTypeDetail, Guid? economicActivityId, string? economicActivityDetail, Guid? CiiuCode, Guid? greatContributorId, Guid? isSelfRetaining, decimal? fee, string? originResources,
              string? emailCorporate, string? electronicInvoiceEmail, string? phoneNumber, Guid? countryId, Guid? departmentId, Guid? cityId, string? address, string? branchAddress,
              string? branchPhoneNumber, Guid? branchDepartmentId, Guid? branchCityId, string? branchContactName, Guid? branchDocumentNumberTypeId, string? branchDocumentNumber, string? branchContactPhone,
              string? branchEmailContact, string? branchPosition, Guid? completed, Guid? statusId, DateTime? statusDate, DateTime? createdOn)
        {
            Id = id;
            Nit = nit;
            CheckDigit = checkDigit;
            CompanyName = companyName;
            CompanyTypeId = companyTypeId;
            SocietyTypeId = societyTypeId;
            SocietyTypeDetail = societyTypeDetail;
            EconomicActivityId = economicActivityId;
            EconomicActivityDetail = economicActivityDetail;
            CIIUCode = CiiuCode;
            GreatContributorId = greatContributorId;
            IsSelfRetaining = isSelfRetaining;
            Fee = fee;
            OriginResources = originResources;
            EmailCorporate = emailCorporate;
            ElectronicInvoiceEmail = electronicInvoiceEmail;
            PhoneNumber = phoneNumber;
            CountryId = countryId;
            DepartmentId = departmentId;
            CityId = cityId;
            Address = address;
            BranchAddress = branchAddress;
            BranchPhoneNumber = branchPhoneNumber;
            BranchDepartmentId = branchDepartmentId;
            BranchCityId = branchCityId;
            BranchContactName = branchContactName;
            BranchDocumentNumberTypeId = branchDocumentNumberTypeId;
            BranchDocumentNumber = branchDocumentNumber;
            BranchContactPhone = branchContactPhone;
            BranchEmailContact = branchEmailContact;
            BranchPosition = branchPosition;
            Completed = completed;
            StatusId = statusId;
            StatusDate = statusDate;
            CreatedOn = createdOn;
        }

        public string? Nit { get; private set; }

        public string? CheckDigit { get; private set; }

        public string? CompanyName { get; private set; }

        public Guid? CompanyTypeId { get; private set; }

        public Guid? SocietyTypeId { get; private set; }

        public string? SocietyTypeDetail { get; private set; }
        public Guid? EconomicActivityId { get; private set; }

        public string? EconomicActivityDetail { get; private set; }

        public Guid? CIIUCode { get; private set; }

        public Guid? GreatContributorId { get; private set; }

        public Guid? IsSelfRetaining { get; private set; }

        public decimal? Fee { get; private set; }

        public string? OriginResources { get; private set; }

        public string? EmailCorporate { get; private set; }

        public string? ElectronicInvoiceEmail { get; private set; }
        public string? PhoneNumber { get; private set; }

        public Guid? CountryId { get; private set; }

        public Guid? DepartmentId { get; private set; }

        public Guid? CityId { get; private set; }

        public string? Address { get; private set; }

        public string? BranchAddress { get; private set; }

        public string? BranchPhoneNumber { get; private set; }

        public Guid? BranchDepartmentId { get; private set; }

        public Guid? BranchCityId { get; private set; }

        public string? BranchContactName { get; private set; }

        public Guid? BranchDocumentNumberTypeId { get; private set; }

        public string? BranchDocumentNumber { get; private set; }

        public string? BranchContactPhone { get; private set; }

        public string? BranchEmailContact { get; private set; }

        public string? BranchPosition { get; private set; }

        public Guid? Completed { get; private set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; private set; }
    }
}