///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Entities
{
    public class CatalogInfo : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Descripton { get; set; } = string.Empty;

        public virtual ICollection<CatalogItemInfo> CatalogItem { get; set; } = new List<CatalogItemInfo>();
    }
}