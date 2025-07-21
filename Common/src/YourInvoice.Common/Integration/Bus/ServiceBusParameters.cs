///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.Bus
{
    public class ServiceBusParameters
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string FullyQualifiedNamespace { get; set; }
        public string ConnectionString { get; set; }
        public bool AuthenticationAD { get; set; }
    }
}