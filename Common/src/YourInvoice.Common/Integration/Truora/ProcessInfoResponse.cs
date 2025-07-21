namespace yourInvoice.Common.Integration.Truora
{
    public class ProcessInfoResponse
    {
        public string process_id { get; set; }
        public string account_id { get; set; }
        public string client_id { get; set; }
        public string flow_id { get; set; }
        public string created_via { get; set; }
        public int flow_version { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string failure_status { get; set; }
        public string declined_reason { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime update_date { get; set; }
        public string ip_address { get; set; }
        public TriggerInfo trigger_info { get; set; }
        public int time_to_live { get; set; }
        public string current_step_type { get; set; }
    }

    public class TriggerInfo
    {
        public string channel_name { get; set; }
        public string channel_type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string media_content_path { get; set; }
        public string trigger_user { get; set; }
        public string Response { get; set; }
        public object Options { get; set; }
    }
}
