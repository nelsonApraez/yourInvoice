///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.ZapSign
{
    public class ZapsignFileResponse
    {
        public int open_id { get; set; }
        public string token { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        public string original_file { get; set; }
        public string signed_file { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_update_at { get; set; }
        public List<ZapsignSignerResponse> signers { get; set; }
    }

    public class ZapsignSignerResponse
    {
        public string token { get; set; }
        public string sign_url { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone_country { get; set; }
        public string phone_number { get; set; }
        public int times_viewed { get; set; }
        public DateTime? last_view_at { get; set; }
        public DateTime? signed_at { get; set; }
        public string resend_attempts { get; set; }
    }
}