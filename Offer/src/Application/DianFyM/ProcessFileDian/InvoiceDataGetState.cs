///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.DianFyM.ProcessFileDian
{
    internal class InvoiceDataGetState
    {
        public Guid InvoiceId { get; set; }
        public bool Event030 { get; set; } = false;

        public bool Event032 { get; set; } = false;

        public bool Event033 { get; set; } = false;

        public bool Event036 { get; set; } = false;

        public bool Event037 { get; set; } = false;

        public bool Event06 { get; set; } = false;

        public bool Event07 { get; set; } = false;

        public bool Endoso { get; set; } = false;

        public bool Tiene_Evento_Pago { get; set; } = false;

        public bool Reclamo { get; set; }
    }
}