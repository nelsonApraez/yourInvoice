///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Infrastructure.Persistence.Configuration
{
    public static class ConstantDataBase
    {
        public static string DateTimeZone => "(dateadd(minute,(-300)-datepart(tzoffset,sysdatetimeoffset()),getdate()))";
        public static string SchemaOffer => "OFT";
        public static string SchemaBuyer => "COM";
        public static string SchemaTrm => "TRM";
        public static string SchemaDbo => "DBO";
        public static string SchemaAdmin => "ADM";
    }
}