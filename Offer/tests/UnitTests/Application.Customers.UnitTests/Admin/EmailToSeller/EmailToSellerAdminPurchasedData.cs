using yourInvoice.Common.Entities;

namespace Application.Customer.UnitTest.Admin.EmailToSeller
{
    public static class EmailToSellerAdminPurchasedData
    {
        public static CatalogItemInfo GetCatalogItemInfo => new CatalogItemInfo
        {
            Descripton = "correo@prueba.com",
            Name = "correo@prueba.com",
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

        public static Dictionary<string, string> GetAttachData => new Dictionary<string, string>()
        {

        };

        public static List<AttachFile> GetAttachFilesData => new List<AttachFile>()
        {

        };

        public static int GetNumberOffer => 370;
        public static string GetNameSeller => "123456789";
        public static List<string> GetEmailsSeller => new List<string>() { "correo@pruebas.com" };
    }
}
