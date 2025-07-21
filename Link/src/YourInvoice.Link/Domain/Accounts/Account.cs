///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;
using yourInvoice.Link.Domain.AccountRoles;

namespace yourInvoice.Link.Domain.Accounts
{
    public class Account : AggregateRoot
    {
        public Account()
        {
        }

        public Account(Guid id, Guid? personTypeId, string? nit, string? digitVerify, string? socialReason, string? name, string? secondName, string? lastName,
            string? secondLastName, Guid? documentTypeId, string? documentNumber, string? email, string? mobileNumber, Guid? mobileCountryId, string? phoneNumber,
            Guid? phoneCountryId, Guid? contactById, string? description, Guid? statusId, DateTime? statusDate, int? time, DateTime? createdOn)
        {
            Id = id;
            PersonTypeId = personTypeId;
            Nit = nit;
            DigitVerify = digitVerify;
            SocialReason = socialReason;
            Name = name;
            SecondName = secondName;
            LastName = lastName;
            SecondLastName = secondLastName;
            DocumentTypeId = documentTypeId;
            DocumentNumber = documentNumber;
            Email = email;
            MobileNumber = mobileNumber;
            MobileCountryId = mobileCountryId;
            PhoneNumber = phoneNumber;
            PhoneCountryId = phoneCountryId;
            ContactById = contactById;
            Description = description;
            StatusId = statusId;
            StatusDate = statusDate;
            Time = time;
            CreatedOn = createdOn;
        }

        public Guid? PersonTypeId { get; set; }

        public string? Nit { get; set; }

        public string? DigitVerify { get; set; }

        public string? SocialReason { get; set; }

        public string? Name { get; set; }

        public string? SecondName { get; set; }

        public string? LastName { get; set; }

        public string? SecondLastName { get; set; }

        public Guid? DocumentTypeId { get; set; }

        public string? DocumentNumber { get; set; }

        public string? Email { get; set; }

        public string? MobileNumber { get; set; }

        public Guid? MobileCountryId { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? PhoneCountryId { get; set; }

        public Guid? ContactById { get; set; }

        public string? Description { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }

        public int? Time { get; set; }

        public virtual ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
    }
}