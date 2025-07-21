///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.MoneyTransfers.Queries;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class MoneyTransferRepository : RepositoryBase<MoneyTransfer>, IMoneyTransferRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISystem system;

        public MoneyTransferRepository(ApplicationDbContext context, ISystem system) : base(context, system)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.system = system;
        }

        public async Task<MoneyTransfer> GetByIdAsync(Guid id) => await _context.MoneyTransfers.SingleOrDefaultAsync(c => c.Id == id);

        public async Task<List<MoneyTransfer>> GetAllByOfferId(Guid offerId)
        {
            return await _context.MoneyTransfers.Where(x => x.OfferId == offerId).ToListAsync();
        }

        public async Task<int> GetCountId(Guid offerId)
        {
            return await _context.MoneyTransfers.Where(x => x.OfferId == offerId).CountAsync();
        }

        public MoneyTransfer Add(MoneyTransfer moneyTransfer) => _context.MoneyTransfers.Add(moneyTransfer).Entity;

        public async Task DeleteAsync(Guid moneyTransferId)
        {
            await _context.MoneyTransfers.Where(x => x.Id == moneyTransferId).ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsByDocumentAsync(string document, Guid offerId, Guid bankId) =>
            await _context.MoneyTransfers.AnyAsync(moneyTransfer => moneyTransfer.DocumentNumber == document && moneyTransfer.OfferId == offerId && moneyTransfer.BankId == bankId);

        public async Task<bool> ExistsByIdAsync(Guid id) => await _context.MoneyTransfers.AnyAsync(moneyTransfer => moneyTransfer.Id == id);

        public async Task<decimal?> TotalAsync(Guid offerId)
        {
            return await _context.MoneyTransfers.Where(x => x.OfferId == offerId && x.Status == true).SumAsync(s => s.Total);
        }

        public async Task<ListDataInfo<BeneficiariesListResponse>> ListAsync(Guid offerId, SearchInfo pagination)
        {
            var query = (from u in _context.MoneyTransfers
                         from cs in _context.CatalogItems.Where(x => x.Id == u.BankId).DefaultIfEmpty()
                         from cm in _context.CatalogItems.Where(x => x.Id == u.DocumentTypeId).DefaultIfEmpty()
                         from ca in _context.CatalogItems.Where(x => x.Id == u.AccountTypeId).DefaultIfEmpty()
                         where
                         ((string.IsNullOrEmpty(pagination.filter) || u.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || cm.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || ca.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || u.Total.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || u.AccountNumber.Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || u.DocumentNumber.Contains(pagination.filter))) &&
                         u.OfferId == offerId &&
                         u.Status == true
                         select new BeneficiariesListResponse
                         {
                             Id = u.Id,
                             Name = u.Name,
                             DocumentType = cm.Name,
                             DocumentNumber = u.DocumentNumber,
                             Bank = cs.Descripton,
                             AccountType = ca.Name,
                             AccountNumber = u.AccountNumber,
                             Total = u.Total.Value,
                         });

            var result = new ListDataInfo<BeneficiariesListResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.Paginate(pagination).ToListAsync()
            };

            return result;
        }

        public async Task<MoneyTransferDocumentResponse> ListToMoneyTransferDocumentAsync(Guid offerId)
        {
            var gobal = System.Globalization.CultureInfo.CreateSpecificCulture("es-CO");

            var query = (from u in _context.MoneyTransfers
                         from cs in _context.CatalogItems.Where(x => x.Id == u.BankId).DefaultIfEmpty()
                         from cm in _context.CatalogItems.Where(x => x.Id == u.DocumentTypeId).DefaultIfEmpty()
                         from ca in _context.CatalogItems.Where(x => x.Id == u.AccountTypeId).DefaultIfEmpty()
                         where
                         u.OfferId == offerId &&
                         u.Status == true
                         select new MoneyTransferDocumentTableContentResponse
                         {
                             Name = u.Name,
                             DocumentType = cm.Name,
                             DocumentNumber = u.DocumentNumber,
                             Bank = cs.Descripton,
                             AccountType = ca.Name,
                             AccountNumber = u.AccountNumber,
                             Total = u.Total.Value.ToString("C0", gobal),
                         });

            MoneyTransferDocumentResponse moneyTransferDocumentResponse = new()
            {
                TableContent = query.ToList(),
                Count = await query.CountAsync(),
                Total = _context.MoneyTransfers.Where(x => x.OfferId == offerId && x.Status == true).Sum(x => x.Total).Value.ToString("C0", gobal)
            };

            return moneyTransferDocumentResponse;
        }

        public async Task<int> CountBeneficiaryAsync(string document, Guid offerId)
        {
            var result = await _context.MoneyTransfers.CountAsync(c => c.OfferId == offerId && c.DocumentNumber == document);

            return result;
        }
    }
}