///*** projectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Persistence.Configuration;

namespace yourInvoice.Offer.Infrastructure.Persistence.Seed
{
    public class CatalogItemSeed : IEntityTypeConfiguration<CatalogItemInfo>
    {
        private readonly Guid createdBy = Guid.Parse("B3822949-5D72-4016-BE39-840F124D95AD");
        private readonly Guid modifiedBy = Guid.Parse("EC036BDD-D5DD-4433-B70C-4CE4D0E2809B");

        public void Configure(EntityTypeBuilder<CatalogItemInfo> builder)
        {
            builder.HasData(
                        new CatalogItemInfo { Id = Guid.Parse("6030B706-0377-45D9-9A9D-DEC4A39F1FF2"), CatalogName = ConstDataBase.InvoiceStatus, Name = "CARGADO", Descripton = "Estados de la factura Cargado", CreatedBy = createdBy, ModifiedBy = modifiedBy }

                        );
        }
    }
}