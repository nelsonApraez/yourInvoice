///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using System.Globalization;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.ValidateInvoicesExcel
{
    public class InvoiceExcelModel
    {
        public string No_factura { get; set; }
        public int Valor_neto_de_pago { get; set; }
        public string Fecha_de_pago { get; set; }

        internal DateTime _fecha_de_pago
        {
            get
            {
                DateTime defaultDate = default;
                try
                {
                    var array = Fecha_de_pago.Split(" ")[0];
                    var gobal = CultureInfo.CreateSpecificCulture("es-CO");

                    if (DateTime.TryParse(Fecha_de_pago, out DateTime dateCurrent))
                    {
                        defaultDate = dateCurrent;
                        Fecha_de_pago = defaultDate.ToString("dd/MM/yyyy", gobal);
                    }
                    else
                    {
                        if (DateTime.TryParse(Fecha_de_pago.Split(" ")[0], out DateTime dateCurrent1))
                        {
                            defaultDate = dateCurrent1;
                            Fecha_de_pago = defaultDate.ToString("dd/MM/yyyy", gobal);
                        }
                        else
                        {
                            var arraySplit = array.Split("/");
                            if (arraySplit.Count() == 3 && int.TryParse(arraySplit[2], out int year) && int.TryParse(arraySplit[1], out int month) && int.TryParse(arraySplit[0], out int day))
                            {
                                try
                                {
                                    // System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-es");

                                    if (month <= 12)
                                    {
                                        defaultDate = new DateTime(year, month, day);
                                    }
                                    else
                                    {
                                        defaultDate = new DateTime(year, day, month);
                                    }
                                    Fecha_de_pago = defaultDate.ToString("dd/MM/yyyy", gobal);
                                }
                                catch (Exception)
                                {
                                    throw new ArgumentException("El formato de la fecha debe ser dd/MM/yyyy");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return defaultDate;
            }
        }
    }
}