///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.ListDocs
{
    public class ListDocsResponse
    {
        public string Name { get; set; }
        public Guid DocumentId { get; set; }
        public bool IsSigned { get; set; }
        public string Size { get; set; }
    }
}