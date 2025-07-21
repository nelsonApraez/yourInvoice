namespace yourInvoice.Link.Application.LinkingProcess.CreateBank
{
    public class Bank
    {
        public Guid Id_GeneralInformation { get; set; }
        public Guid? BankReference { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BankProduct { get; set; }
        public Guid? DepartmentState { get; set; }
        public Guid? City { get; set; }
        public Guid? Completed { get; set; }

    }
}
