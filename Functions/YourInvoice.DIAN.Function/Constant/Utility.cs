///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.DIAN.Function.Constant
{
    public static class Utility
    {
        public static DateTime DateTimeCO
        {
            get
            {
                try
                {
                    DateTime timeUtc = DateTime.UtcNow;
                    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                    DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                    return cstTime;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return DateTime.Now;
                }
            }
        }
    }
}