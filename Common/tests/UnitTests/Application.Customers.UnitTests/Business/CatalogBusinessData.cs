///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;

namespace yourInvoice.Common.UnitTests.Business
{
    public static class CatalogBusinessData
    {
        public static List<CatalogItemInfo> GetCatalogItemInfo
        {
            get => new List<CatalogItemInfo>
            {
                new CatalogItemInfo() {  CatalogName="TIPODOCUMENTO", Name="Cédula"  },
                new CatalogItemInfo() {  CatalogName="TIPODOCUMENTO", Name="Passaporte"  },
                new CatalogItemInfo() {  CatalogName="TIPODOCUMENTO", Name="Tarjeta de Identidad"  }
            };
        }
    }
}