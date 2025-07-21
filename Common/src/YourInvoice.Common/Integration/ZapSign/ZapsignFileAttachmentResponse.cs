///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.ZapSign
{
    public class ZapsignFileAttachmentResponse
    {
        public int open_id { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public string original_file { get; set; }
        public object signed_file { get; set; }
    }
}