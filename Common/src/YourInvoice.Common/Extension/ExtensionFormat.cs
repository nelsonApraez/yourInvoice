///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using System.Globalization;
using System.Text.RegularExpressions;

namespace yourInvoice.Common.Extension
{
    public static class ExtensionFormat
    {
        public static string DateFullCumston(this DateTime? date)
        {
            return date is null ? "sin fecha" : date.Value.ToString("dd/MMM/yyyy", new CultureInfo("es-ES", false)).Replace(".", "");
        }

        public static string DateddMMyyyy(this DateTime? date)
        {
            return date is null ? "sin fecha" : date.Value.ToString("dd/MM/yyyy", new CultureInfo("es-ES", false)).Replace(".", "");
        }

        public static string DateddMMMyyyy(this DateTime? date)
        {
            return date is null ? "sin fecha" : date.Value.ToString("dd/MMM/yyyy", new CultureInfo("es-ES", false)).Replace(".", "");
        }

        public static string DateddMMMyyyy(this DateTime date)
        {
            return date.ToString("dd/MMM/yyyy", new CultureInfo("es-ES", false)).Replace(".", "");
        }

        public static string DateddMMMyyyyHHmm(this DateTime date)
        {
            return date.ToString("dd/MMM/yyyy HH:mm", new CultureInfo("es-ES", false)).Replace(".", "");
        }

        public static DateTime GetDateAnyFormat(string dateWithoutFormat)
        {
            if (string.IsNullOrEmpty(dateWithoutFormat) || !dateWithoutFormat.Contains("/"))
            {
                return DateTime.MinValue;
            }
            dateWithoutFormat = dateWithoutFormat.Trim().Split(" ")[0];
            string[] formats = { "M/d/yyyy", "d/M/yyyy", "yyyy/M/d", "yyyy/d/M" };
            var date = DateTime.ParseExact(dateWithoutFormat, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            return date;
        }

        public static DateTime DateTimeCO()
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
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                return DateTime.Now;
            }
        }

        public static DateTime DateTimeCOddmmyyyy()
        {
            try
            {
                CultureInfo cultura = new CultureInfo("es-CO");
                Thread.CurrentThread.CurrentCulture = cultura;
                Thread.CurrentThread.CurrentUICulture = cultura;
                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                return cstTime;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                return DateTime.Now;
            }
        }

        public static string ToMegaByte(this long size)
        {
            return string.Concat((size / Math.Pow(1024, 2)).ToString().AsSpan(0, 4), " mb");
        }

        public static string ToMegaByte(this double size)
        {
            return string.Concat((size / Math.Pow(1024, 2)).ToString().AsSpan(0, 4), " mb");
        }

        public static string ToMegaByte(this int size)
        {
            return string.Concat((size / Math.Pow(1024, 2)).ToString().AsSpan(0, 4), " mb");
        }

        public static long NumberLongWithoutPeriodOrCommas(string dataNumber)
        {
            if (string.IsNullOrEmpty(dataNumber))
            {
                return 0;
            }
            int positionPoint = dataNumber.IndexOf(".");
            int positionCommas = dataNumber.IndexOf(",");
            long number = 0;
            if (positionPoint <= 0 && positionCommas <= 0)
            {
                long.TryParse(dataNumber, out number);
                return number;
            }
            int lengthNumber = positionPoint > positionCommas ? positionPoint : positionCommas;
            dataNumber = dataNumber.Trim().Substring(0, lengthNumber);

            long.TryParse(dataNumber, out number);
            return number;
        }

        public static int NumberIntWithoutPeriodOrCommas(string dataNumber)
        {
            if (string.IsNullOrEmpty(dataNumber))
            {
                return 0;
            }
            int positionPoint = dataNumber.IndexOf(".");
            int positionCommas = dataNumber.IndexOf(",");
            int number = 0;
            if (positionPoint <= 0 && positionCommas <= 0)
            {
                int.TryParse(dataNumber, out number);
                return number;
            }
            int lengthNumber = positionPoint > positionCommas ? positionPoint : positionCommas;
            dataNumber = dataNumber.Trim().Substring(0, lengthNumber);
            int.TryParse(dataNumber, out number);
            return number;
        }

        public static string UpperFirtsLetter(this string cadena)
        {
            if (string.IsNullOrEmpty(cadena))
                return cadena;

            char[] caracteres = cadena.ToCharArray();
            caracteres[0] = char.ToUpper(caracteres[0]);
            return new string(caracteres);
        }

        public static decimal OnlyNumberTypeDecimal(string dataNumber)
        {
            if (string.IsNullOrEmpty(dataNumber))
            {
                return 0;
            }
            decimal number = 0;
            int positionPoint = dataNumber.LastIndexOf(".");
            int positionCommas = dataNumber.LastIndexOf(",");
            int lengthNumber = positionPoint > positionCommas ? positionPoint : positionCommas;
            if (lengthNumber > 0)
            {
                var isDecimal = dataNumber.Substring(lengthNumber).Length;
                dataNumber = isDecimal <= 3 ? dataNumber.Substring(0, lengthNumber) : dataNumber;
            }
            string patron = @"(?:- *)?\d+(?:\.\d+)?";
            Regex regex = new Regex(patron);
            string[] result = regex.Matches(dataNumber)
                                       .OfType<Match>()
                                       .Select(m => m.Value)
                                       .ToArray();
            var data = string.Join("", result);
            data = data.Replace(".", "").Replace(",", "");
            decimal.TryParse(data, out number);
            return number;
        }

        public static decimal DecimalWhitCommaPoint(string dataNumber)
        {
            if (string.IsNullOrEmpty(dataNumber))
            {
                return 0;
            }
            decimal number = 0;
            decimal.TryParse(dataNumber, out number);
            return number;
        }

        public static string GetNameMonth()
        {
            string monthName = NameMonth(numberMonth: 0, dateMonth: default);
            return monthName;
        }

        public static string GetNameMonth(int numberMonth)
        {
            string monthName = NameMonth(numberMonth: numberMonth, dateMonth: default);
            return monthName;
        }

        public static string GetNameMonth(DateTime dateMonth)
        {
            string monthName = NameMonth(numberMonth: 0, dateMonth: dateMonth);
            return monthName;
        }

        private static string NameMonth(int numberMonth = 0, DateTime dateMonth = default)
        {
            CultureInfo spanishCulture = new CultureInfo("es-ES");
            int month = GetValueMonth(numberMonth, dateMonth);
            string monthName = spanishCulture.DateTimeFormat.GetMonthName(month);
            return monthName;
        }

        private static int GetValueMonth(int numberMonth = 0, DateTime dateMonth = default)
        {
            if (numberMonth == 0 && dateMonth == default) { return DateTimeCO().Month; }
            if (numberMonth == 0) { return numberMonth; }
            return dateMonth.Month;
        }

        public static string GetNumberInLetters(int number)
        {
            string[] unidades = { "", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
            string[] decenas = { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
            string[] especiales = { "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };

            if (number == 0)
                return "cero";

            if (number < 10)
                return unidades[number];

            if (number < 20 && number > 10)
                return especiales[number - 11];

            if (number >= 10 && number < 30)
                return (number == 20) ? "veinte" : "veinti" + unidades[number - 20];

            if (number < 100)
            {
                int unidad = number % 10;
                int decena = number / 10;
                return decenas[decena] + (unidad > 0 ? " y " + unidades[unidad] : "");
            }

            return "";
        }
    }
}