///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class DocumentInfo : ModelBase
    {
        public Guid OfferId { get; set; }

        public Guid InvoiceId { get; set; }

        public Guid? TypeId { get; set; }

        public bool? IsSigned { get; set; }

        public string Url { get; set; }
    }
}