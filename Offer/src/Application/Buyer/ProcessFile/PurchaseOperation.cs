///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.ProcessFile
{
    public class PurchaseOperation
    {
        public string NombreComprador { get; set; }
        public string TipoDeDocumentoComprador { get; set; }
        public string NumeroDeDocumentoComprador { get; set; }
        public DateTime FechaDeCompra { get; set; }
        public DateTime FechaFinal { get; set; }
        public int NoTran { get; set; }
        public string FacturaNo { get; set; }
        public string Fraccionamiento { get; set; }
        public string NombrePagador { get; set; }
        public string NumeroDeDocumentoPagador { get; set; }
        public string NombreVendedor { get; set; }
        public string NumeroDeDocumentoVendedor { get; set; }
        public decimal TasaEAPorcentaje { get; set; }
        public int DiasOper { get; set; }
        public long VrCompra { get; set; }
        public long VrFuturo { get; set; }
        public string Reasignacion { get; set; }
        public string DineroNuevo { get; set; }
        public Guid PayerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid InvoiceDispersionId { get; set; }
        public DateTime FechaOperacion { get; set; }
        public DateTime FechaEsperada { get; set; }
        public int TransaccionPadre { get; set; }
    }
}