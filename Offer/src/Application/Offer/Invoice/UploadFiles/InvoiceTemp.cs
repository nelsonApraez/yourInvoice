///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.UploadFiles
{
    public record InvoiceTemp
    {
        public Dictionary<string, byte[]> XmlFile { get; set; }
        public Dictionary<string, byte[]> PdfFile { get; set; }
        public string Number { get; set; }
        public string ZipName { get; set; }
        public string Cufe { get; set; }
        public string EmitDate { get; set; }
        public string DueDate { get; set; }
        public string Total { get; set; }
        public string TaxAmount { get; set; }
        public string Trm { get; set; }
        public string MoneyType { get; set; }
        public string NitEmisor { get; set; }
        public string NitReceptor { get; set; }
        public string Url { get; set; }
    }
}