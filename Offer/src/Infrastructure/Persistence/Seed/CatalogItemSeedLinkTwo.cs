///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Persistence.Configuration;

namespace yourInvoice.Offer.Infrastructure.Persistence.Seed
{
    public class CatalogItemSeedLinkTwo : IEntityTypeConfiguration<CatalogItemInfo>
    {
        private readonly Guid createdBy = Guid.Parse("B3822949-5D72-4016-BE39-840F124D95AD");
        private readonly Guid modifiedBy = Guid.Parse("EC036BDD-D5DD-4433-B70C-4CE4D0E2809B");

        public void Configure(EntityTypeBuilder<CatalogItemInfo> builder)
        {
            builder.HasData(
                    new CatalogItemInfo { Id = Guid.Parse("64D17A42-F9D3-42D2-8670-DEBCAD2C2746"), Order = 1, CatalogName = ConstDataBase.ParagraphDeclarationSignature, Name = "Declaración origen de fondor", Descripton = "<p class=\"parrafo\">Declaro bajo la gravedad del juramento que mi patrimonio y los recursos con los que realizo mis actividades económicas, así como con los que realizo las operaciones por intermedio de yourInvoice S.A., provienen de actividades lícitas, en especial de las siguientes fuentes.</p>", CreatedBy = createdBy, ModifiedBy = modifiedBy }
                    

                );
        }
    }
}