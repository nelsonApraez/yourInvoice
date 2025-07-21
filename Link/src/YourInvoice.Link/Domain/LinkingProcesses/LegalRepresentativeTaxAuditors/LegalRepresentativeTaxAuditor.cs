///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors
{
    public class LegalRepresentativeTaxAuditor : AggregateRoot
    {
        public LegalRepresentativeTaxAuditor()
        {
        }

        public LegalRepresentativeTaxAuditor(Guid id, Guid id_LegalGeneralInformation, string? firstName, string? secondName, string? lastName, string? secondLastName, Guid? documentTypeId, string? documentNumber, DateTime? expeditionDate
            , string? expeditionCountry, string? email, string? homeAddress, string? phone, Guid? departmentState, Guid? city, string? taxAuditorFirstName, string? taxAuditorSecondName, string? taxAuditorLastName
            , string? taxAuditorSecondLastName, Guid? taxAuditorDocumentTypeId, string? taxAuditorDocumentNumber, string? taxAuditorPhoneNumber
            , Guid? completed, Guid? statusId, DateTime? statusDate, DateTime? createdOn)
        {
            Id = id;
            Id_LegalGeneralInformation = id_LegalGeneralInformation;
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            SecondLastName = secondLastName;
            DocumentTypeId = documentTypeId;
            DocumentNumber = documentNumber;
            ExpeditionDate = expeditionDate;
            ExpeditionCountry = expeditionCountry;
            Email = email;
            HomeAddress = homeAddress;
            Phone = phone;
            DepartmentState = departmentState;
            City = city;
            TaxAuditorFirstName = taxAuditorFirstName;
            TaxAuditorSecondName = taxAuditorSecondName;
            TaxAuditorSecondLastName = taxAuditorSecondLastName;
            TaxAuditorLastName = taxAuditorLastName;
            TaxAuditorDocumentTypeId = taxAuditorDocumentTypeId;
            TaxAuditorDocumentNumber = taxAuditorDocumentNumber;
            TaxAuditorPhoneNumber = taxAuditorPhoneNumber;
            Completed = completed;
            StatusId = statusId;
            StatusDate = statusDate;
            CreatedOn = createdOn;
        }

        public Guid Id_LegalGeneralInformation { get; private set; }

        public string? FirstName { get; private set; }

        public string? SecondName { get; private set; }
        public string? LastName { get; private set; }

        public string? SecondLastName { get; private set; }

        public Guid? DocumentTypeId { get; private set; }

        public string? DocumentNumber { get; set; }

        public DateTime? ExpeditionDate { get; private set; }

        public string? ExpeditionCountry { get; private set; }

        public string? Email { get; private set; }

        public string? HomeAddress { get; private set; }

        public string? Phone { get; private set; }

        public Guid? DepartmentState { get; private set; }

        public Guid? City { get; private set; }

        public string? TaxAuditorFirstName { get; private set; }

        public string? TaxAuditorSecondName { get; private set; }
        public string? TaxAuditorLastName { get; private set; }

        public string? TaxAuditorSecondLastName { get; private set; }

        public string? TaxAuditorPhoneNumber { get; set; }

        public Guid? TaxAuditorDocumentTypeId { get; private set; }

        public string? TaxAuditorDocumentNumber { get; set; }

        public Guid? Completed { get; private set; }

        public Guid? StatusId { get; private set; }

        public DateTime? StatusDate { get; private set; }
    }
}