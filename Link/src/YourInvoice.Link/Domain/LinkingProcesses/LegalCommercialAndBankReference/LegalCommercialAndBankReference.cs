
using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference
{
    public class LegalCommercialAndBankReference : AggregateRoot
    {
        public LegalCommercialAndBankReference()
        {
        }

        public LegalCommercialAndBankReference(Guid id,
            Guid? idLegalGeneralInformation,
             string? commercialReference,
             string? phoneNumberCommercial,
             Guid? departmentStateCommercial,
             Guid? cityCommercial,
             string? bankReference,
             string? phoneNumberBank,
             Guid? departmentStateBank,
             Guid? cityBank,
            Guid? completed,
            DateTime? createdOn,
            Guid? statusId,
            DateTime? statusDate)
        {
            Id = id;
            Id_LegalGeneralInformation = idLegalGeneralInformation;
            CommercialReference = commercialReference;
            PhoneNumberCommercial = phoneNumberCommercial;
            DepartmentStateCommercial = departmentStateCommercial;
            CityCommercial = cityCommercial;
            BankReference = bankReference;
            PhoneNumberBank = phoneNumberBank;
            DepartmentStateBank = departmentStateBank;
            CityBank = cityBank;
            Completed = completed;
            CreatedOn = createdOn;
            StatusId = statusId;
            StatusDate = statusDate;
            CreatedOn = createdOn;
        }

        public Guid? Id_LegalGeneralInformation { get; set; }
        public string? CommercialReference { get; set; }
        public string? PhoneNumberCommercial { get; set; }
        public Guid? DepartmentStateCommercial { get; set; }
        public Guid? CityCommercial { get; set; }
        public string? BankReference { get; set; }
        public string? PhoneNumberBank { get; set; }
        public Guid? DepartmentStateBank { get; set; }
        public Guid? CityBank { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
    }
}
