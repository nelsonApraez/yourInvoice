///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Offers.Queries;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class OfferRepository : RepositoryBase<Domain.Offer>, IOfferRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISystem system;

        public OfferRepository(ApplicationDbContext context, ISystem system) : base(context, system)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.system = system;
        }

        public Domain.Offer Add(Domain.Offer offer) => _context.Offers.Add(offer).Entity;

        public void Delete(Domain.Offer offer) => _context.Offers.Remove(offer);

        public void Update(Domain.Offer offer) => _context.Offers.Update(offer);

        public async Task<bool> ExistsAsync(Guid id) => await _context.Offers.AnyAsync(offer => offer.Id == id);

        public async Task<Domain.Offer> GetByIdAsync(Guid id) => await _context.Offers.SingleOrDefaultAsync(c => c.Id == id);

        public async Task<Domain.Offer> GetByConsecutiveAsync(int consecutive) => await _context.Offers.SingleOrDefaultAsync(c => c.Consecutive == consecutive);

        public async Task<List<Domain.Offer>> GetAll() => await _context.Offers.ToListAsync();

        public async Task DeleteAsync(Guid offerId)
        {
            await _context.Offers.Where(x => x.Id == offerId).ExecuteDeleteAsync();
        }

        public async Task<Domain.Offer> GetOfferAsync(Guid offerId)
        {
            var result = await _context.Offers.FirstOrDefaultAsync(x => x.Id == offerId);

            return result ?? new();
        }

        public async Task<GetOfferResponse> GetByIdWithNamesAsync(Guid offerId)
        {
            decimal? total = await _context.Invoices
                .Where(i => i.OfferId == offerId && i.Status == true)
                .SumAsync(i => i.MoneyTypeId == CatalogCode_InvoiceMoneyType.USD ? i.Total * i.Trm : i.Total);
            int count = await _context.Invoices
               .Where(i => i.OfferId == offerId && i.Status == true)
               .CountAsync();

            var result = (from u in _context.Offers
                          from p in _context.Payers.Where(x => x.Id == u.PayerId).DefaultIfEmpty()
                          from s in _context.Users.Where(x => x.Id == u.UserId).DefaultIfEmpty()
                          from c in _context.CatalogItems.Where(x => x.Id == u.StatusId).DefaultIfEmpty()
                          where
                         u.Id == offerId &&
                         u.Status == true
                          select new GetOfferResponse
                          (
                              p.Nit,
                              p.Name,
                              s.Name,
                              c.Name,
                              c.Id,
                              count,
                              total,
                              u.Consecutive
                          ));

            return await result.SingleOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Invoice>> GetInvoiceProcessConfirmAsync(Guid offerId)
        {
            return await _context.Invoices.Where(c => c.OfferId == offerId && c.StatusId == CatalogCode_InvoiceStatus.Approved).ToListAsync();
        }

        public async Task<ListDataInfo<ListAllOfferResponse>> ListAllByUserAsync(SearchInfo pagination, Guid userId = default)
        {
            if (pagination.ColumnOrder == "dateCreation")
            {
                pagination.ColumnOrder = "DateCreationOrder";
            }

            var userIsEmpty = userId == Guid.Empty;
            var query = (from OF in _context.Offers
                         from S in _context.CatalogItems.Where(x => x.Id == OF.StatusId).DefaultIfEmpty()
                         from P in _context.Payers.Where(x => x.Id == OF.PayerId).DefaultIfEmpty()
                         where
                         ((string.IsNullOrEmpty(pagination.filter) || P.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || S.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || OF.Consecutive.ToString().Contains(pagination.filter))) &&
                         (userIsEmpty || OF.UserId == userId) && OF.Status == true
                         select new ListAllOfferResponse
                         {
                             OfferId = OF.Id,
                             NoOffer = OF.Consecutive,
                             DateCreation = OF.CreatedOn.DateddMMMyyyy(),
                             DateCreationOrder = OF.CreatedOn,
                             Status = S.Name,
                             StatusId = S.Id,
                             NamePayer = P.Name
                         });
            var resultInvoices = new ListDataInfo<ListAllOfferResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.Paginate(pagination).ToListAsync()
            };
            return resultInvoices;
        }

        public async Task<DetailOfferResponse> DetailAsync(Guid offerId, Guid userId = default)
        {
            var userIsEmpty = userId == Guid.Empty;
            var query = await (from O in _context.Offers
                               join I in _context.Invoices on O.Id equals I.OfferId
                               from S in _context.CatalogItems.Where(x => x.Id == O.StatusId).DefaultIfEmpty()
                               from P in _context.Payers.Where(x => x.Id == O.PayerId).DefaultIfEmpty()
                               where (O.Id == offerId && (userIsEmpty || O.UserId == userId))
                               group new { O, I, S, P } by O.Consecutive into g
                               select new DetailOfferResponse
                               {
                                   OfferId = g.FirstOrDefault().O.Consecutive,
                                   BusinessName = g.FirstOrDefault().P.Name,
                                   PayerNit = g.FirstOrDefault().P.Nit,
                                   Status = g.FirstOrDefault().S.Name,
                                   StatusId = g.FirstOrDefault().S.Id,
                                   AmountinvoiceUploadedSuccessfully = g.Count(c => c.I.StatusId != CatalogCode_InvoiceStatus.Loaded),
                                   TotalValueOffer = g.Where(c => c.I.StatusId != CatalogCode_InvoiceStatus.Loaded).Sum(s => s.I.Total),
                                   beneficiaries = (from B in _context.MoneyTransfers where B.OfferId == offerId select B.Id).Count()
                               }).FirstOrDefaultAsync();

            if (query == null)
            {
                query = await (from O in _context.Offers
                               from S in _context.CatalogItems.Where(x => x.Id == O.StatusId).DefaultIfEmpty()
                               from P in _context.Payers.Where(x => x.Id == O.PayerId).DefaultIfEmpty()
                               where (O.Id == offerId && (userIsEmpty || O.UserId == userId))
                               group new { O, S, P } by O.Consecutive into g
                               select new DetailOfferResponse
                               {
                                   OfferId = g.FirstOrDefault().O.Consecutive,
                                   BusinessName = g.FirstOrDefault().P.Name,
                                   PayerNit = g.FirstOrDefault().P.Nit,
                                   Status = g.FirstOrDefault().S.Name,
                                   StatusId = g.FirstOrDefault().S.Id
                               }).FirstOrDefaultAsync();
            }

            return query;
        }

        public async Task<ProcessFileValidationResponse> GetDataValidateStateOfferEnabledAsync(int offerNumber, string nroInvoice, string documentPayer)
        {
            var dataValidation = await (from O in _context.Offers
                                        join I in _context.Invoices on O.Id equals I.OfferId
                                        join P in _context.Payers on O.PayerId equals P.Id
                                        where O.Consecutive == offerNumber && I.Number == nroInvoice && P.Nit.Trim() == documentPayer.Trim()
                                        select new ProcessFileValidationResponse
                                        {
                                            PayerId = O.PayerId,
                                            PayerNit = P.Nit,
                                        }).FirstOrDefaultAsync();
            return dataValidation;
        }

        public async Task<Guid> GetIdOfferAsync(int offerNumber)
        {
            var result = await _context.Offers.FirstOrDefaultAsync(c => c.Consecutive == offerNumber);
            return result?.Id ?? Guid.Empty;
        }

        public async Task<bool> OfferIsInProgressAsync(Guid offerId)
        {
            return await _context.Offers.AnyAsync(x => x.Id == offerId && x.StatusId == CatalogCode_OfferStatus.InProgress);
        }

        public async Task<bool> OfferIsPurchasedAsync(int consecutive)
        {
            return await _context.Offers.AnyAsync(x => x.Consecutive == consecutive && x.StatusId == CatalogCode_OfferStatus.Purchased);
        }

        public async Task<bool> OfferIsInProgressByInvoiceIdAsync(Guid invoiceId)
        {
            return await (from o in _context.Offers
                          from i in _context.Invoices.Where(x => x.OfferId == o.Id).DefaultIfEmpty()
                          where i.Id == invoiceId && o.StatusId == CatalogCode_OfferStatus.InProgress
                          select o).AnyAsync();
        }

        public async Task<bool> OfferIsInProgressByBeneficiaryIdAsync(Guid beneficiaryId)
        {
            return await (from o in _context.Offers
                          from b in _context.MoneyTransfers.Where(x => x.OfferId == o.Id).DefaultIfEmpty()
                          where b.Id == beneficiaryId && o.StatusId == CatalogCode_OfferStatus.InProgress
                          select o).AnyAsync();
        }

        public async Task<decimal?> TotalOfferAsync(Guid offerId)
        {
            var query = await _context.Invoices.Where(x => x.OfferId == offerId && x.StatusId != CatalogCode_InvoiceStatus.Loaded).SumAsync(s => s.Total);

            return query;
        }
    }
}