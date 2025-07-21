///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class InvoiceEventInfo : ModelBase
    {
        public Guid InvoiceId { get; set; }

        public bool Event030 { get; set; } = false;

        public bool Event032 { get; set; } = false;

        public bool Event033 { get; set; } = false;

        public bool Event036 { get; set; } = false;

        public bool Event037 { get; set; } = false;

        public bool? Event06 { get; set; } = null;

        public bool? Event07 { get; set; } = null;
        public bool? Claim { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}