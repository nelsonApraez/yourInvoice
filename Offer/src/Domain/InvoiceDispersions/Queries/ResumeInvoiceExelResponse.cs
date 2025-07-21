///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.InvoiceDispersions.Queries
{
    public class ResumeInvoiceExelResponse
    {
        public string Nombre_Comprador { get; set; }
        public int Nro_Transaccion { get; set; }
        public string Nro_Factura { get; set; }
        public DateTime Fecha_de_Vencimiento { get; set; }
        public long Total_Factura { get; set; }
        public long Valor_Pago_Neto { get; set; }
        public string Dia_Tasa { get; set; }
        public DateTime Fecha_de_Pago { get; set; }
    }
}