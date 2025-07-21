///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.ExcelModule;
using System.Data;

namespace yourInvoice.Common.UnitTest
{
    public class ExcelBusinessUnitTest
    {
        private readonly Mock<ICatalogBusiness> mockICatalogRepository = new Mock<ICatalogBusiness>();

        [Fact]
        public void Excel_ShouldGenerateComponent()
        {
            //Preparar la data
            var dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Sold", typeof(int));

            dataTable.Rows.Add("Cheesecake", 14);
            dataTable.Rows.Add("Medovik", 6);
            dataTable.Rows.Add("Muffin", 10);

            // Act
            var ms = ExcelBusiness.Generate(dataTable);

            #region Descomentar para generar el archivo fisico si se requiere

            //*****************************************************************************************//
            //*******PRUEBA ALMACENAR EXCEL EN DIRECTORIO DE EJECUCI�N PARA VISUALIZAR EL ARCHIVO******//
            //*****************************************************************************************//

            //string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //FileStream file = new FileStream($"{path}\\fileExcel.xlsx", FileMode.Create, FileAccess.Write);
            //ms.WriteTo(file);
            //file.Close();
            //ms.Close();

            #endregion Descomentar para generar el archivo fisico si se requiere

            // Assert
            Assert.NotNull(ms);
        }
    }
}