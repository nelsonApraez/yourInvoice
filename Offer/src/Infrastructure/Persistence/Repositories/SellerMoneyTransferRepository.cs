///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.SellerMoneyTransfers;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class SellerMoneyTransferRepository : ISellerMoneyTransferRepository
    {
        private readonly ApplicationDbContext _context;

        public SellerMoneyTransferRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(SellerMoneyTransfer sellerMoneyTransfer)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid sellerMoneyTransferId)
        {
            throw new NotImplementedException();
        }

        public void Update(SellerMoneyTransfer sellerMoneyTransfer)
        {
            throw new NotImplementedException();
        }
    }
}