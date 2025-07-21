///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.ZapSign
{
    public class ZapsignFileDetailResponse
    {
        public bool sandbox { get; set; }
        public string external_id { get; set; }
        public int open_id { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public string folder_path { get; set; }
        public string status { get; set; }
        public string lang { get; set; }
        public string original_file { get; set; }
        public string signed_file { get; set; }
        public List<ZapsignFileDetailExtraDocResponse> extra_docs { get; set; }
        public string created_through { get; set; }
        public bool deleted { get; set; }
        public DateTime? deleted_at { get; set; }
        public bool signed_file_only_finished { get; set; }
        public bool disable_signer_emails { get; set; }
        public string brand_logo { get; set; }
        public string brand_primary_color { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_update_at { get; set; }
        public ZapsignFileDetailCreatedByResponse created_by { get; set; }
        public List<ZapsignFileDetailSignerResponse> signers { get; set; }
    }

    public class ZapsignFileDetailSignerResponse
    {
        public string external_id { get; set; }
        public string token { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        public bool lock_name { get; set; }
        public string email { get; set; }
        public bool lock_email { get; set; }
        public string phone_country { get; set; }
        public string phone_number { get; set; }
        public bool lock_phone { get; set; }
        public int times_viewed { get; set; }
        public DateTime? last_view_at { get; set; }
        public DateTime? signed_at { get; set; }
        public string auth_mode { get; set; }
        public string qualification { get; set; }
        public bool require_selfie_photo { get; set; }
        public bool require_document_photo { get; set; }
        public string geo_latitude { get; set; }
        public string geo_longitude { get; set; }
        public string redirect_link { get; set; }
        public ZapsignFileDetailResendAttemptResponse resend_attempts { get; set; }
    }

    public class ZapsignFileDetailResendAttemptResponse
    {
        public int whatsapp { get; set; }
        public int email { get; set; }
        public int sms { get; set; }
    }

    public class ZapsignFileDetailExtraDocResponse
    {
        public int open_id { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public string original_file { get; set; }
        public string signed_file { get; set; }
    }

    public class ZapsignFileDetailCreatedByResponse
    {
        public string email { get; set; }
    }
}