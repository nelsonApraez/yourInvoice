///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace Application.Customer.UnitTest.User
{
    public static class UserData
    {
        public static yourInvoice.Offer.Domain.Users.User GetUser => new yourInvoice.Offer.Domain.Users.User(
           Guid.Parse("9bd5a44f-2fdd-488e-9b76-878bce147fdc"), 2, "890104521", "test name", Guid.NewGuid(), "72335847",
                "Cali", "Gerente", "Calle de prueba 123", "3015433443", "test@test.com", "Cali",
                Guid.NewGuid(), Guid.NewGuid(), "Licores del valle", "97654234", "5", "8725426273", "Cali", "Cali",
                true, DateTime.UtcNow, Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid()
            );
    }
}