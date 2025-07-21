///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Offer.Domain
{
    public interface ISystem
    {
        User User { get; set; }

        void Set(User user);
    }
}