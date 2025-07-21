///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations
{
    public class GeneralInformation : AggregateRoot
    {
        public GeneralInformation()
        {
        }

        public GeneralInformation(Guid id, string firstName, string? secondName, string lastName, string? secondLastName, Guid documentTypeId, string documentNumber,
            DateTime? expeditionDate, Guid? expeditionCountry, Guid? economicActivity, Guid? secondaryEconomicActivity, string? email, string? phoneNumber,
            string? movilPhoneNumber, Guid? departmentState, Guid? city, string? address, 
            string? phoneCorrespondence, Guid? departmentStateCorrespondence, Guid? cityCorrespondence, string? addressCorrespondence, 
            Guid? completed, Guid? linkStatus, Guid? statusId, DateTime? statusDate,DateTime? createdOn)
        {
            Id = id;
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            SecondLastName = secondLastName;
            DocumentTypeId = documentTypeId;
            DocumentNumber = documentNumber;
            ExpeditionDate = expeditionDate;
            ExpeditionCountry = expeditionCountry;
            EconomicActivity = economicActivity;
            SecondaryEconomicActivity = secondaryEconomicActivity;
            Email = email;
            PhoneNumber = phoneNumber;
            MovilPhoneNumber = movilPhoneNumber;
            DepartmentState = departmentState;
            City = city;
            Address = address;
            PhoneCorrespondence = phoneCorrespondence;
            DepartmentStateCorrespondence = departmentStateCorrespondence;
            CityCorrespondence = cityCorrespondence;
            AddressCorrespondence = addressCorrespondence;
            Completed = completed;
            LinkStatus = linkStatus;
            StatusId = statusId;
            StatusDate = statusDate;
            CreatedOn = createdOn;
        }

        public string FirstName { get; set; }

        public string? SecondName { get; set; }

        public string LastName { get; set; }

        public string? SecondLastName { get; set; }

        public Guid DocumentTypeId { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime? ExpeditionDate { get; set; }

        public Guid? ExpeditionCountry { get; set; }

        public Guid? EconomicActivity { get; set; }

        public Guid? SecondaryEconomicActivity { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? MovilPhoneNumber { get; set; }

        public Guid? DepartmentState { get; set; }

        public Guid? City { get; set; }

        public string? Address { get; set; }

        public string? PhoneCorrespondence { get; set; }

        public Guid? DepartmentStateCorrespondence { get; set; }

        public Guid? CityCorrespondence { get; set; }

        public string? AddressCorrespondence { get; set; }

        public Guid? Completed { get; set; }
        public Guid? LinkStatus { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}