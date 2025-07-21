///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalRepresentativeTaxAuditor
{
    public class LegalRepresentativeTax
    {
        public Guid Id_LegalGeneralInformation { get; set; }

        public string? FirstName { get; set; }

        public string? SecondName { get; set; }
        public string? LastName { get; set; }

        public string? SecondLastName { get; set; }

        public Guid? DocumentTypeId { get; set; }

        public string? DocumentNumber { get; set; }

        public DateTime? ExpeditionDate { get; set; }

        public string? ExpeditionCountry { get; set; }

        public string? Email { get; set; }

        public string? HomeAddress { get; set; }

        public string? Phone { get; set; }

        public Guid? DepartmentState { get; set; }

        public Guid? City { get; set; }

        public string? TaxAuditorFirstName { get; set; }

        public string? TaxAuditorSecondName { get; set; }
        public string? TaxAuditorLastName { get; set; }

        public string? TaxAuditorSecondLastName { get; set; }

        public string? TaxAuditorPhoneNumber { get; set; }

        public Guid? TaxAuditorDocumentTypeId { get; set; }

        public string? TaxAuditorDocumentNumber { get; set; }

        public Guid? Completed { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}