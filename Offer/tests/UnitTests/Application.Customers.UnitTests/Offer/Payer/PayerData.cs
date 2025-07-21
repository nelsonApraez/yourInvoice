///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace Application.Customer.UnitTest.Offer.Payer
{
    using yourInvoice.Offer.Domain.Payers;

    public static class PayerData
    {
        public static string GetNit => "800130305";

        public static List<Payer> GetPayers =>
            new List<Payer>
            {
                new Payer(
                       Guid.Parse("FA3B0387-7C4C-46FA-9DFE-000F42FD2707")
                        ,"800130305"
                        ,"0"
                        ,"FLORES LA MANA SAS"
                        ,"FLORES LA MANA SAS"
                        ,"CL 19 5 30 OF2201 EDIFICIO BD BACATA"
                        ,"BOGOTA"
                        ,false
                        ,"BOGOTA"
                        ,false
                        ,"16683030"
                        ,"floreslamana@floreslamana.com"
                        ,"CL 19 5 30 OF2201 EDIFICIO BD BACATA"
                    )
            };

        public static List<Payer> GetPayersEmpty => new List<Payer>();
    }
}