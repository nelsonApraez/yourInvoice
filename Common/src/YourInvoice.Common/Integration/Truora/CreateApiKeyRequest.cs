
namespace yourInvoice.Common.Integration.Truora
{
    public class CreateApiKeyRequest
    {
        public string key_type { get; set; } = "web";
        public int api_key_version { get; set; } = 1;
        public string country { get; set; } = "ALL";
        public string grant { get; set; } = "digital-identity";
        public string redirect_url { get; set; } = "{YOUR_REDIRECT_URL}";
        public string flow_id { get; set; } = "{YOUR_FLOW_ID}";
        public string account_id { get; set; }
    }
}
