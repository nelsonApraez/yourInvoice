///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class InvoiceInfo : ModelBase
    {
        public Guid OfferId { get; set; }

        public string Number { get; set; }

        public string ZipName { get; set; }

        public string CUFE { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? EmitDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? Total { get; set; }

        public Guid? MoneyTypeId { get; set; }

        public double? Trm { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime? NegotiationDate { get; set; }

        public decimal? NegotiationTotal { get; set; }
    }
}