///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class AccountInfo : ModelBase
    {
        public Guid? PersonTypeId { get; set; }

        public string Nit { get; set; }

        public string DigitVerify { get; set; }

        public string SocialReason { get; set; }

        public string Name { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string SecondLastName { get; set; }

        public Guid? DocumentTypeId { get; set; }

        public string DocumentNumber { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public Guid? MobileCountryId { get; set; }

        public string PhoneNumber { get; set; }

        public Guid? PhoneCountryId { get; set; }

        public Guid? ContactById { get; set; }

        public string Description { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }

        public int? Time { get; set; }
    }
}