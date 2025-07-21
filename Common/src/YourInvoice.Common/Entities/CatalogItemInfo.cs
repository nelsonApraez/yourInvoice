///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Entities
{
    public class CatalogItemInfo : BaseEntity
    {
        public Guid? ParentId { get; set; }

        public string CatalogName { get; set; } = string.Empty;

        public string Descripton { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int? Order { get; set; }

        public virtual CatalogInfo Catalog { get; set; }
    }
}