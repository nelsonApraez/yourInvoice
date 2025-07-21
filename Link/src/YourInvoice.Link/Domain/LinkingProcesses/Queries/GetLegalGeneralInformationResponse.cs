///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetLegalGeneralInformationResponse
    {
        public Guid Id { get; set; }
        public string? Nit { get; set; }

        public string? CheckDigit { get; set; }

        public string? CompanyName { get; set; }

        public Guid? CompanyTypeId { get; set; }

        public string TypeCompanyDescription { get; set; }

        public Guid? SocietyTypeId { get; set; }
        public string TypeSocietyDescription { get; set; }

        public string? SocietyTypeDetail { get; set; }
        public Guid? EconomicActivityId { get; set; }
        public string? EconomicActivityDescription { get; set; }
        public string? EconomicActivityDetail { get; set; }

        public Guid? CIIUCode { get; set; }

        public Guid? GreatContributorId { get; set; }

        public Guid? IsSelfRetaining { get; set; }

        public decimal? Fee { get; set; }

        public string? OriginResources { get; set; }

        public string? EmailCorporate { get; set; }

        public string? ElectronicInvoiceEmail { get; set; }
        public string? PhoneNumber { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? CityId { get; set; }

        public string? Address { get; set; }

        public string? BranchAddress { get; set; }

        public string? BranchPhoneNumber { get; set; }

        public Guid? BranchDepartmentId { get; set; }

        public Guid? BranchCityId { get; set; }

        public string? BranchContactName { get; set; }
        public Guid? BranchDocumentNumberTypeId { get; set; }
        public string? BranchDocumentNumber { get; set; }
        public string? BranchContactPhone { get; set; }
        public string? BranchEmailContact { get; set; }

        public string? BranchPosition { get; set; }

        public Guid? Completed { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}