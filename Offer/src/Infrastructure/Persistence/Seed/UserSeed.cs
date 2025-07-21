///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Offer.Infrastructure.Persistence.Seed
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        private Guid userTmp = Guid.Parse("0950F21A-28E2-43E4-AE7D-DA89CA7F1512");

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User(Guid.Parse("71D81124-3B1B-449A-BB40-B25490A7C47E"), 0, string.Empty, "USUARIO DEL SISTEMA", Guid.Empty, "0000000000", string.Empty, string.Empty, string.Empty, "11223344", "builtin@yourInvoice.co", string.Empty, Guid.Parse("c77e3528-e582-435d-a527-812c5b57f9cb"), Guid.Parse("bffba02f-b413-4f18-9df6-79715b541e84"), string.Empty, "0000000000", string.Empty, string.Empty, string.Empty, string.Empty, true, ExtensionFormat.DateTimeCO(), userTmp, ExtensionFormat.DateTimeCO(), userTmp),
                new User(Guid.Parse("6e1b4dd4-b82e-43b1-bb3c-c50d51d1a1d2"), 0, string.Empty, "Darío Fernando Escobar Risueño", Guid.Parse("fbb3b4ea-3d57-417a-83c9-22243e775fe5"), "98389248", "lugar de expedicion documento", "Trabajo", "Direccion", "3002134827", "dfescobar@yourInvoice.co", string.Empty, Guid.Parse("5c2f2b7e-edda-4e5f-b875-08072e206a83"), Guid.Parse("0326A0F0-9DDC-412A-B11A-B9D862B51E40"), "Darío Fernando Escobar Risueño", "0000000000", string.Empty, string.Empty, string.Empty, string.Empty, true, ExtensionFormat.DateTimeCO(), userTmp, ExtensionFormat.DateTimeCO(), userTmp)
                );
        }
    }
}