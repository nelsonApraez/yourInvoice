///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Application.Documents.Common;
using yourInvoice.Offer.Domain.Documents;

namespace Application.Customer.UnitTest.Documents
{
    public static class DocumentData
    {
        public static DocumentByOfferInvoiceRequest GetDocumentByOfferInvoiceRequest
            => new DocumentByOfferInvoiceRequest(Guid.Parse("60BA55D1-A205-4A6F-8A62-F0C9FB44AB32"), Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA3"), "pdf");

        public static DocumentByOfferInvoiceRequest GetDocumentByOfferInvoiceRequestExtensionTypeFileEmpty
           => new DocumentByOfferInvoiceRequest(Guid.Parse("60BA55D1-A205-4A6F-8A62-F0C9FB44AB32"), Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA3"), string.Empty);

        public static List<Document> GetDocuments
         => new List<Document>
         {
             new Document(
                  Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA1")
                 ,Guid.Parse("60BA55D1-A205-4A6F-8A62-F0C9FB44AB32")
                 ,Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA3")
                 ,"FACTURA.pdf"
                 ,Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA3")
                 ,false
                 ,"https://yourInvoicestorage.blob.core.windows.net/yourInvoicecontainer/FEVN6445.pdf"
             ),
             new Document(
                  Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA1")
                 ,Guid.Parse("60BA55D1-A205-4A6F-8A62-F0C9FB44AB32")
                 ,Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA3")
                 ,"FACTURA.xml"
                 ,Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA3")
                 ,false
                 ,"https://yourInvoicestorage.blob.core.windows.net/yourInvoicecontainer/FEVN6445.xml"
             )
         };

        public static List<Document> GetDocumentsEmpy => new List<Document>();

        public static string GetUrlToken => @"https://yourInvoicestorage.blob.core.windows.net/yourInvoicecontainer/FEVN6445.xml?sv=2023-08-03&st=2023-11-08T20%3A14%3A49Z&se=2023-11-08T20%3A24%3A49Z&sr=b&sp=r&sig=gL1QM%2BsOxKSEKBYLfu1YEllM7DlDf0CGqNAU3fj7yx4%3D";
    }
}