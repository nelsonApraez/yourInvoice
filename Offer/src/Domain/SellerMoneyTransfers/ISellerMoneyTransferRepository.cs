///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.SellerMoneyTransfers
{
    public interface ISellerMoneyTransferRepository
    {
        void Add(SellerMoneyTransfer sellerMoneyTransfer);

        void Update(SellerMoneyTransfer sellerMoneyTransfer);

        void Delete(Guid sellerMoneyTransferId);
    }
}