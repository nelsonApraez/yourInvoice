///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Infrastructure.Persistence.Configuration
{
    public static class ConstantDataBase
    {
        public static string DateTimeZone => "(dateadd(minute,(-300)-datepart(tzoffset,sysdatetimeoffset()),getdate()))";
        public static string SchemaBinding => "VIN";
    }
}