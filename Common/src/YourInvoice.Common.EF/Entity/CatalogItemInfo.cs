///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class CatalogItemInfo : ModelBase
    {
        public Guid? ParentId { get; set; }

        public string CatalogName { get; set; }

        public string Descripton { get; set; }

        public string Name { get; set; }

        public int? Order { get; set; }
    }
}