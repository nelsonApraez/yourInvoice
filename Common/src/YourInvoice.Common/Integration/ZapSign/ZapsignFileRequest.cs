///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;

namespace yourInvoice.Common.Integration.ZapSign
{
    public class ZapsignFileRequest
    {
        public string name { get; set; }
        public string externalId { get; set; }
        public string base64_pdf { get; set; }
        internal string lang = "es";
        internal bool disable_signer_emails = true;
        internal bool signed_file_only_finished = false;
        internal DateTime date_limit_to_sign = ExtensionFormat.DateTimeCO().AddDays(1);
        public List<ZapsignSignerRequest> Signers { get; set; }
    }

    public class ZapsignSignerRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        internal string auth_mode = "assinaturaTela";
        internal string lang = "es";
        internal bool send_automatic_email = false;
        internal bool send_automatic_whatsapp = false;
        internal bool require_selfie_photo = false;
        internal bool require_document_photo = false;
        internal string selfie_validation_type = "none";
    }
}