namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalCommercialAndBankReference
{
    public class References
    {
        public Guid Id_LegalGeneralInformation { get; set; }
        public string? CommercialReference { get; set; }
        public string? PhoneNumberCommercial { get; set; }
        public Guid? DepartmentStateCommercial { get; set; }
        public Guid? CityCommercial { get; set; }
        public string? BankReference { get; set; }
        public string? PhoneNumberBank { get; set; }
        public Guid? DepartmentStateBank { get; set; }
        public Guid? CityBank { get; set; }
        public Guid? Completed { get; set; }
    }
}
