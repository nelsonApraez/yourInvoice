//*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class GetBankResponse

    {
        public Guid Id { get; set; }
        public Guid? Id_GeneralInformation { get; set; }
        public Guid? BankReference { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BankProduct { get; set; }
        public Guid? DepartmentState { get; set; }
        public Guid? City { get; set; }
        public Guid? Completed { get; set; }
        public Guid? StatusId { get; set; }

    }
}
