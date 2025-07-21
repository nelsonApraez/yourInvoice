///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Documents;

namespace Application.Customer.UnitTest.Admin.SendSummary
{
    public static class SendSummaryData
    {
        public static List<Document> GetDocuments => new List<Document>()
        {
             new Document{ Url="https://CMSblobfiledllo.blob.core.windows.net/yourInvoicecontainer/storage/729/Documents/Other/ResumenFacturas.xml"},
             new Document{ Url="https://CMSblobfiledllo.blob.core.windows.net/yourInvoicecontainer/storage/729/Documents/Other/prueba.pdf"},
             new Document{ Url="https://CMSblobfiledllo.blob.core.windows.net/yourInvoicecontainer/storage/729/Documents/Other/ResumenFacturas.xlsx"},
        };

        public static MemoryStream GetStream
        {
            get
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes("This is a sample string");
                MemoryStream ms = new MemoryStream();
                ms.Write(data, 0, data.Length);
                ms.Close();
                return ms;
            }
        }
    }
}