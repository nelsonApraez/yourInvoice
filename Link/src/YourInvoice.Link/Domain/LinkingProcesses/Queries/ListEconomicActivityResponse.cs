///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Domain.LinkingProcesses.Queries
{
    public class ListEconomicActivityResponse
    {
        public Guid Id { get; set; }

        public int OrderRegister { get; set; }

        public string NameOrder { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public string Status { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }

    }
}