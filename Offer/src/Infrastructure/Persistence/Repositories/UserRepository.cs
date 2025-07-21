///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISystem system;

        public UserRepository(ApplicationDbContext context, ISystem system) : base(context, system)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.system = system;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Delete(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(Guid userId) => await _context.Users.SingleOrDefaultAsync(c => c.Id == userId);

        public async Task<Guid?> GetPersonTypeByIdAsync(Guid userId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(c => c.Id == userId);
            return account.PersonTypeId;
        }

        public async Task<ListDataInfo<OfferListResponse>> ListOffersAsync(Guid buyerId, SearchInfo pagination, int timeEnable)
        {
            var query = (from invoDis in _context.InvoiceDispersions
                         from of in _context.Offers.Where(x => x.Consecutive == invoDis.OfferNumber).DefaultIfEmpty()
                         from csi in _context.CatalogItems.Where(x => x.Id == invoDis.StatusId).DefaultIfEmpty()
                         from use in _context.Users.Where(x => x.Id == invoDis.SellerId).DefaultIfEmpty()
                         from upa in _context.Payers.Where(x => x.Id == invoDis.PayerId).DefaultIfEmpty()
                         from docs in _context.Documents.Where(x => x.RelatedId == buyerId && x.OfferId == of.Id && x.TypeId == CatalogCode_DocumentType.TransferSupportBuyer).DefaultIfEmpty()
                         where
                         ((string.IsNullOrEmpty(pagination.filter) || invoDis.OfferNumber.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || invoDis.CurrentValue.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || invoDis.FutureValue.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || csi.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || use.Company.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || upa.Name.ToLower().Contains(pagination.filter.ToLower()))) &&
                         invoDis.BuyerId == buyerId &&
                         invoDis.Status == true &&
                         invoDis.PurchaseDate >= ExtensionFormat.DateTimeCO().AddHours(-timeEnable) && //Filtrar las ultimas 24 horas
                         (invoDis.StatusId == CatalogCode_InvoiceDispersionStatus.PendingPurchase || invoDis.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased)
                         select new OfferListResponse
                         {
                             OfferId = of.Id,
                             OfferNumber = invoDis.OfferNumber,
                             Status = csi.Name,
                             StatusId = csi.Id,
                             SellerName = use.Company,
                             PayerName = upa.Name,
                             OrderDate = invoDis.PurchaseDate,
                             CreationDate = invoDis.PurchaseDate.DateddMMMyyyyHHmm(),
                             Rate = (invoDis.Rate * 100).ToString("N2") + "% EA",
                             Term = invoDis.OperationDays.ToString() + " días",
                             PurchaseValue = invoDis.CurrentValue,
                             FutureValue = invoDis.FutureValue,
                             NewMoney = _context.InvoiceDispersions.Where(x => buyerId == x.BuyerId && x.OfferNumber == invoDis.OfferNumber).Any(c => c.NewMoney),
                             DocId = docs.Id
                         });

            var data = new ListDataInfo<OfferListResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.ToListAsync()
            };

            return data;
        }

        public async Task<ListDataInfo<OfferListResponse>> ListOffersHistoryAsync(Guid buyerId, SearchInfo pagination)
        {
            pagination.PageSize = 1000;
            var query = (from invoDis in _context.InvoiceDispersions
                         from o in _context.Offers.Where(x => x.Consecutive == invoDis.OfferNumber).DefaultIfEmpty()
                         from cs in _context.CatalogItems.Where(x => x.Id == invoDis.StatusId).DefaultIfEmpty()
                         from us in _context.Users.Where(x => x.Id == invoDis.SellerId).DefaultIfEmpty()
                         from up in _context.Payers.Where(x => x.Id == invoDis.PayerId).DefaultIfEmpty()
                         from doc in _context.Documents.Where(x => x.RelatedId == buyerId && x.OfferId == o.Id && x.TypeId == CatalogCode_DocumentType.TransferSupportBuyer).DefaultIfEmpty()
                         where
                         ((string.IsNullOrEmpty(pagination.filter) || invoDis.OfferNumber.ToString().Contains(pagination.filter)) ||
                          (string.IsNullOrEmpty(pagination.filter) || invoDis.CurrentValue.ToString().Contains(pagination.filter)) ||
                          (string.IsNullOrEmpty(pagination.filter) || invoDis.FutureValue.ToString().Contains(pagination.filter)) ||
                          (string.IsNullOrEmpty(pagination.filter) || cs.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || us.Company.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || up.Name.ToLower().Contains(pagination.filter.ToLower()))) &&
                         invoDis.BuyerId == buyerId &&
                         invoDis.Status == true &&
                         invoDis.StatusId != CatalogCode_InvoiceDispersionStatus.PendingPurchase //todas menos las pendientes de compra
                         select new OfferListResponse
                         {
                             OfferId = o.Id,
                             OfferNumber = invoDis.OfferNumber,
                             Status = cs.Name,
                             StatusId = cs.Id,
                             SellerName = us.Company,
                             PayerName = up.Name,
                             OrderDate = invoDis.PurchaseDate,
                             CreationDate = invoDis.PurchaseDate.DateddMMMyyyyHHmm(),
                             Rate = (invoDis.Rate * 100).ToString("N2") + "% EA",
                             Term = invoDis.OperationDays.ToString() + " días",
                             PurchaseValue = invoDis.CurrentValue,
                             FutureValue = invoDis.FutureValue,
                             NewMoney = invoDis.NewMoney,
                             DocId = doc.Id
                         });

            var result = new ListDataInfo<OfferListResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.ToListAsync()
            };

            return result;
        }

        public async Task<string> GetEmailRoleAsync(Guid roleId)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.RoleId == roleId);

            return result.Email ?? string.Empty;
        }

        public async Task<string> GetEmailSellerByOfferAsync(int offerId)
        {
            var emailSeller = await (from U in _context.Users
                                     join ID in _context.InvoiceDispersions on U.Id equals ID.SellerId
                                     where ID.OfferNumber == offerId
                                     select U.Email).FirstOrDefaultAsync();
            return emailSeller;
        }

        public async Task<User> GetByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(c => c.Email.Trim() == email.Trim());

        public async Task<List<GetRoleResponse>> GetRoleAsync(Guid userId)
        {
            var result = await (from u in _context.Users
                                join r in _context.CatalogItems on u.RoleId equals r.Id
                                where u.Id == userId
                                select new GetRoleResponse
                                {
                                    RoleId = u.RoleId,
                                    RoleName = r.Name,
                                }).ToListAsync();
            return result;
        }

        public async Task<User> GetUserAsync(string documentNumber, Guid userRoleId)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.DocumentNumber.Trim() == documentNumber.Trim() && u.RoleId == userRoleId);

            return result;
        }
    }
}