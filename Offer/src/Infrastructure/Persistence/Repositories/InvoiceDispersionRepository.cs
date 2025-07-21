///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;
using yourInvoice.Offer.Domain.Offers.Queries;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class InvoiceDispersionRepository : IInvoiceDispersionRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceDispersionRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddAsync(IEnumerable<InvoiceDispersion> invoicesDispersion)
        {
            await _context.InvoiceDispersions.AddRangeAsync(invoicesDispersion);
            return true;
        }

        public async Task<InvoiceDispersion> GetByOfferNumberAndBuyerIdAsync(int offerNumber, Guid buyerId)
        {
            return await _context.InvoiceDispersions.Where(c => c.OfferNumber == offerNumber && c.BuyerId == buyerId).FirstOrDefaultAsync();
        }

        public async Task<InvoiceDispersion> GetByTransactionNumberAsync(int transactionNumber)
        {
            return await _context.InvoiceDispersions.Where(c => c.TransactionNumber == transactionNumber).FirstOrDefaultAsync();
        }

        public async Task<List<InvoiceDispersion>> FindByOfferNumberAndBuyerIdAsync(int offerNumber, Guid buyerId)
        {
            return await _context.InvoiceDispersions.Where(c => c.OfferNumber == offerNumber && c.BuyerId == buyerId).ToListAsync();
        }

        public async Task ChangeStatusInvoiceDispersionPurchasedAsync(List<InvoiceDispersion> invoiceDispersions)
        {
            await _context.InvoiceDispersions
                   .Where(c => invoiceDispersions.Select(s => s.Id).Contains(c.Id))
                   .ExecuteUpdateAsync(p => p
                   .SetProperty(u => u.StatusId, CatalogCode_InvoiceDispersionStatus.Purchased));
        }

        public async Task<bool> UpdateAsync(List<InvoiceDispersion> invoiceDispersion)
        {
            await _context.InvoiceDispersions
                          .Where(c => invoiceDispersion.Select(s => s.Id).Contains(c.Id))
                          .ExecuteUpdateAsync(p => p
                          .SetProperty(u => u.StatusId, invoiceDispersion.FirstOrDefault().StatusId)
                          .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                          .SetProperty(u => u.ModifiedBy, invoiceDispersion.FirstOrDefault().PayerId));

            return true;
        }

        public async Task<IEnumerable<InvoiceDispersion>> GetAllDefeatedAsync(DateTime expirationDate, Guid statusRejected)
        {
            var result = await _context.InvoiceDispersions.Where(c => c.ExpirationDate < expirationDate && c.StatusId == statusRejected).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<InvoiceDispersion>> GetFilesWithoutProcessAsync()
        {
            var result = await _context.InvoiceDispersions.Where(c => c.Status == false).ToListAsync();

            return result;
        }

        public async Task<DataValidationProcessFileResponse> ValidateExistsAsync(int offerNumber, int noTransaction, string documentNumberBuyer)
        {
            var result = await (from ID in _context.InvoiceDispersions
                                join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                                join U in _context.Users on ID.BuyerId equals U.Id
                                where ID.OfferNumber == offerNumber && ID.Status == true && ID.TransactionNumber == noTransaction
                                && U.DocumentNumber.Trim() == documentNumberBuyer.Trim() && U.RoleId == CatalogCode_UserRole.Buyer
                                select new DataValidationProcessFileResponse
                                {
                                    InvoiceDispersionId = ID.Id,
                                    InvoiceDispersionStatusId = ID.StatusId
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<int> CountTransactionAsync(int offerNumber, int noTransaction, string documentNumberBuyer)
        {
            var result = await (from ID in _context.InvoiceDispersions
                                join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                                join U in _context.Users on ID.BuyerId equals U.Id
                                where ID.OfferNumber == offerNumber && ID.Status == true && ID.TransactionNumber == noTransaction
                                && U.DocumentNumber.Trim() == documentNumberBuyer.Trim() && U.RoleId == CatalogCode_UserRole.Buyer
                                select new DataValidationProcessFileResponse
                                {
                                    InvoiceDispersionId = ID.Id,
                                }).CountAsync();
            return result;
        }

        public async Task<DataValidationProcessFileResponse> ValidateExistsInvoiceAsync(int offerNumber, string facturaNo, string documentNumberBuyer)
        {
            var result = await (from ID in _context.InvoiceDispersions
                                join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                                join U in _context.Users on ID.BuyerId equals U.Id
                                where ID.OfferNumber == offerNumber && ID.Status == true && ID.InvoiceNumber.Trim() == facturaNo.Trim()
                                && U.DocumentNumber.Trim() == documentNumberBuyer.Trim() && U.RoleId == CatalogCode_UserRole.Buyer
                                select new DataValidationProcessFileResponse
                                {
                                    InvoiceDispersionId = ID.Id,
                                    InvoiceDispersionStatusId = ID.StatusId
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> UpdateReassignmentAsync(List<InvoiceDispersion> invoiceDispersions)
        {
            await _context.InvoiceDispersions
                        .Where(c => invoiceDispersions.Select(s => s.Id).Contains(c.Id))
                        .ExecuteUpdateAsync(p => p
                        .SetProperty(u => u.Status, invoiceDispersions.FirstOrDefault().Status)
                        .SetProperty(u => u.Reallocation, invoiceDispersions.FirstOrDefault().Reallocation)
                        .SetProperty(u => u.ExpirationDate, invoiceDispersions.FirstOrDefault().ExpirationDate)
                        .SetProperty(u => u.StatusId, invoiceDispersions.FirstOrDefault().StatusId));

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="numberOffer"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResumeResponse> GetResumeAsync(int numberOffer, Guid userId = default)
        {
            var userIsEmpty = userId == Guid.Empty;
            var query = await (from ID in _context.InvoiceDispersions
                               join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                               from ST in _context.CatalogItems.Where(x => x.Id == O.StatusId).DefaultIfEmpty()
                               from STI in _context.CatalogItems.Where(x => x.Id == ID.StatusId).DefaultIfEmpty()
                               join U in _context.Users on O.UserId equals U.Id
                               join S in _context.Users on ID.SellerId equals S.Id
                               join P in _context.Payers on ID.PayerId equals P.Id
                               where (ID.OfferNumber == numberOffer && ID.Status == true &&
                                    (userIsEmpty || ID.BuyerId == userId)) &&
                                    ID.StatusId != CatalogCode_InvoiceDispersionStatus.Canceled &&
                                    ID.StatusId != CatalogCode_InvoiceDispersionStatus.CanceledModified &&
                                    ID.StatusId != CatalogCode_InvoiceDispersionStatus.Rejected &&
                                    ID.Reallocation != ConstantCode_Rellocation.A &&
                                    ID.Reallocation != ConstantCode_Rellocation.M
                               group new { ID.OfferNumber, OferId = O.Id, ID.OperationDays, ID.Rate, ID.ExpirationDate, ID.CurrentValue, ID.FutureValue, ID.NewMoney, NameSaller = S.Company, NamePayer = P.Name, O.Id, StatusOffer = STI.Name, StatusOfferId = STI.Id, ID.OperationDate } by ID.OfferNumber into g
                               select new ResumeResponse
                               {
                                   NroOffer = g.FirstOrDefault().OfferNumber,
                                   OfferId = g.FirstOrDefault().OferId,
                                   NameSaler = g.FirstOrDefault().NameSaller,
                                   NamePayer = g.FirstOrDefault().NamePayer,
                                   OperationDay = g.FirstOrDefault().OperationDays,
                                   Rate = (g.FirstOrDefault().Rate * 100).ToString("N2"),
                                   ExpirationDate = g.FirstOrDefault().ExpirationDate,
                                   CurrentValue = g.Sum(s => s.CurrentValue),
                                   FutureValue = g.Sum(s => s.FutureValue),
                                   RecordValue = Convert.ToInt64(g.Where(c => c.NewMoney).Sum(s => s.CurrentValue)),
                                   Status = g.FirstOrDefault().StatusOffer,
                                   StatusId = g.FirstOrDefault().StatusOfferId,
                                   NewMoneyOffer = _context.InvoiceDispersions.Where(x => userId == x.BuyerId && x.OfferNumber == numberOffer).Any(c => c.NewMoney),
                                   HasDocument = _context.Documents.Any(a => a.OfferId == g.FirstOrDefault().OferId && (a.TypeId == CatalogCode_DocumentType.TransferSupportBuyer) && (a.RelatedId == userId)),
                                   OperationDate = g.FirstOrDefault().OperationDate.ToString("dd/MM/yyyy HH:mm")
                               }).FirstOrDefaultAsync();

            return query;
        }

        public async Task<HeaderTransactionResponse> GetHeaderTransactionAsync(int transactionId)
        {
            var query = await (from ID in _context.InvoiceDispersions
                               join S in _context.Users on ID.SellerId equals S.Id
                               join P in _context.Payers on ID.PayerId equals P.Id
                               from cs in _context.CatalogItems.Where(x => x.Id == ID.StatusId).DefaultIfEmpty()
                               where (ID.TransactionNumber == transactionId)
                               select new HeaderTransactionResponse
                               {
                                   TransactioNumber = ID.TransactionNumber,
                                   PayerName = P.Name,
                                   SellerName = S.Company,
                                   StatusId = ID.StatusId,
                                   Date = $"{cs.Name} - {CalculateMesagge(ID.StatusId, ID.PurchaseDate, ID.ExpirationDate)}"
                               }).FirstOrDefaultAsync();

            return query;
        }

        private static string CalculateMesagge(Guid statusId, DateTime PurchaseDate, DateTime ExpirationDate)
        {
            if (statusId == CatalogCode_InvoiceDispersionStatus.Purchased)
            {
                return PurchaseDate.DateddMMMyyyyHHmm();
            }
            else
            {
                return $"Expira dentro de {CalculateMissingHours(ExpirationDate)}";
            }
        }

        private static string CalculateMissingHours(DateTime? dateTarget)
        {
            if (dateTarget == null)
            {
                return "El campo ModifiedOn se encuentra vacio";
            }

            // Obtiene la hora actual en UTC
            DateTime horaUtc = DateTime.UtcNow;

            // Define la zona horaria de Bogotá (GMT-5)
            TimeZoneInfo zonaHorariaBogota = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");

            // Convierte la hora UTC a la hora local de Bogotá
            DateTime horaBogota = TimeZoneInfo.ConvertTimeFromUtc(horaUtc, zonaHorariaBogota);

            TimeSpan diference = (DateTime)dateTarget - horaBogota;

            if (diference.TotalHours < 0)
                return "Ya ha pasado";

            int hours = (int)diference.TotalHours;
            int minutes = diference.Minutes;

            return $"{hours}:{minutes}";
        }

        public async Task<HeaderTransactionResponse> GetHeaderOfferAsync(int offerId)
        {
            var query = await (from ID in _context.InvoiceDispersions
                               join S in _context.Users on ID.SellerId equals S.Id
                               join P in _context.Payers on ID.PayerId equals P.Id
                               where (ID.OfferNumber == offerId)
                               select new HeaderTransactionResponse
                               {
                                   SellerEmail = S.Email,
                                   PayerName = P.Name,
                                   SellerName = S.Company,
                               }).FirstOrDefaultAsync();

            return query;
        }

        public async Task<List<ListTransactionsResponse>> ListTransactionsAsync(int transactionId)
        {
            var query = await (from ID in _context.InvoiceDispersions
                               where ID.TransactionNumber == transactionId && ID.Status == true
                               select new ListTransactionsResponse
                               {
                                   Document = ID.InvoiceNumber,
                                   CurrentValue = ID.CurrentValue,
                                   FutureValue = ID.FutureValue
                               }).ToListAsync();

            return query;
        }

        public async Task<ListDataInfo<ResumeInvoiceResponse>> ResumeInvoiceAsync(SearchInfo pagination, int numberOffer, Guid userId = default)
        {
            var userIsEmpty = userId == Guid.Empty;
            var query = (from ID in _context.InvoiceDispersions
                         join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                         where ((string.IsNullOrEmpty(pagination.filter) || ID.InvoiceNumber.Contains(pagination.filter))
                         && ID.OfferNumber == numberOffer && (userIsEmpty || ID.BuyerId == userId)) && ID.Status == true
                         select new ResumeInvoiceResponse
                         {
                             InvoiceNumber = ID.InvoiceNumber,
                             ExpirationDate = _context.Invoices.Where(x => x.OfferId == O.Id && x.Number == ID.InvoiceNumber && x.Status == true).FirstOrDefault().DueDate.Value.DateddMMMyyyy(),
                             CurrentValue = ID.CurrentValue,
                             FutureValue = ID.FutureValue,
                             DayRate = Convert.ToString(ID.OperationDays) + "/" + (ID.Rate * 100).ToString("N2") + "%",
                             PayDay = ID.EndDate.DateddMMMyyyy()
                         }); ;

            var result = new ListDataInfo<ResumeInvoiceResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.Paginate(pagination).ToListAsync()
            };

            return result;
        }

        public async Task<List<ResumeInvoiceExelResponse>> ResumeInvoiceExelAsync(int numberOffer, Guid buyerId, Guid statusId)
        {
            var result = await (from ID in _context.InvoiceDispersions
                                join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                                join U in _context.Users on O.UserId equals U.Id
                                where (ID.OfferNumber == numberOffer && ID.BuyerId == buyerId && ID.StatusId == statusId && ID.Status == true)
                                select new ResumeInvoiceExelResponse
                                {
                                    Nombre_Comprador = U.Name,
                                    Nro_Transaccion = ID.TransactionNumber,
                                    Nro_Factura = ID.InvoiceNumber,
                                    Fecha_de_Vencimiento = ID.ExpirationDate,
                                    Total_Factura = ID.CurrentValue,
                                    Valor_Pago_Neto = ID.FutureValue,
                                    Dia_Tasa = Convert.ToString(ID.OperationDays) + "/" + Convert.ToString(ID.Rate) + "%",
                                    Fecha_de_Pago = ID.EndDate
                                }).ToListAsync();

            return result;
        }

        public async Task<bool> ChangeStatusToRejectInvoiceAsync(int numberOffer, Guid buyerId, Guid statusId)
        {
            await _context.InvoiceDispersions
                       .Where(c => c.OfferNumber == numberOffer && c.BuyerId == buyerId && c.Status == true)
                       .ExecuteUpdateAsync(p => p
                       .SetProperty(u => u.ModifiedBy, buyerId)
                       .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                       .SetProperty(u => u.StatusId, statusId));

            return true;
        }

        public async Task<int> GetPurchasePercentageAsync(int numberOffer, Guid userId = default)
        {
            var userIsEmpty = userId == Guid.Empty;
            var result = await (from ID in _context.InvoiceDispersions
                                join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                                where ID.OfferNumber == numberOffer && (userIsEmpty || O.UserId == userId) && ID.Status == true
                                group new { ID.CurrentValue } by ID.OfferNumber into g
                                select new
                                {
                                    PurchasePercentage = Convert.ToInt32((_context.InvoiceDispersions
                                                                        .Where(c => c.OfferNumber == numberOffer && c.Status == true && c.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased)
                                                                        .Sum(s => s.CurrentValue) * 100) / g.Sum(s => s.CurrentValue))
                                }).FirstOrDefaultAsync();

            return result?.PurchasePercentage ?? 0;
        }

        public async Task<bool> ThereIsNoMissingMoneyTransferDocument(int numberOffer, Guid userId = default)
        {
            var userIsEmpty = userId == Guid.Empty;
            var result = await (from ID in _context.InvoiceDispersions
                                join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                                from D in _context.Documents.Where(c => c.OfferId == O.Id && c.TypeId == CatalogCode_DocumentType.TransferSupportBuyer).DefaultIfEmpty()
                                where (userIsEmpty || O.UserId == userId) && ID.OfferNumber == numberOffer && ID.NewMoney && string.IsNullOrEmpty(D.Name) && ID.Status == true
                                select ID.InvoiceNumber).AnyAsync();
            return !result;
        }

        public async Task<Guid> ValidateExistsSellerAsync(int numberOffer, string documentNumberSeller)
        {
            var result = await (from O in _context.Offers
                                join U in _context.Users on O.UserId equals U.Id
                                where U.CompanyNit.Trim() == documentNumberSeller.Trim() && O.Consecutive == numberOffer
                                select U.Id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<OfferListResponse>> ListToPurchaseCertificateDocument(Guid buyerId, int offerNumber)
        {
            var query = (from invoDis in _context.InvoiceDispersions
                         from o in _context.Offers.Where(x => x.Consecutive == invoDis.OfferNumber).DefaultIfEmpty()
                         from cs in _context.CatalogItems.Where(x => x.Id == invoDis.StatusId).DefaultIfEmpty()
                         from us in _context.Users.Where(x => x.Id == invoDis.SellerId).DefaultIfEmpty()
                         from up in _context.Payers.Where(x => x.Id == invoDis.PayerId).DefaultIfEmpty()
                         where
                         invoDis.OfferNumber == offerNumber &&
                         invoDis.BuyerId == buyerId &&
                         invoDis.Status == true &&
                         (invoDis.StatusId == CatalogCode_InvoiceDispersionStatus.PendingPurchase || invoDis.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased)
                         select new OfferListResponse
                         {
                             OfferId = o.Id,
                             Status = cs.Name,
                             SellerName = us.Company,
                             PayerName = up.Name,
                             CreationDate = invoDis.PurchaseDate.DateddMMMyyyy(),
                             Rate = (invoDis.Rate * 100).ToString("N2"),
                             Term = invoDis.OperationDays.ToString(),
                             PurchaseValue = invoDis.CurrentValue,
                             FutureValue = invoDis.FutureValue,
                             EndDate = invoDis.EndDate.DateddMMMyyyy(),
                             TransactionNumber = invoDis.TransactionNumber,
                             InvoiceNumber = invoDis.InvoiceNumber + "/" + invoDis.Division
                         });

            return query.ToList();
        }

        public long GetTotalPurchased(Guid buyerId, int offerNumber)
        {
            var result = _context.InvoiceDispersions.Where(c => c.BuyerId == buyerId && c.OfferNumber == offerNumber && c.Status == true && !c.NewMoney).Sum(x => x.CurrentValue);

            return result;
        }

        public bool IsNotNewMoneyOrMixed(Guid buyerId, int offerNumber)
        {
            return _context.InvoiceDispersions.Any(c => c.BuyerId == buyerId && c.OfferNumber == offerNumber && c.Status == true && !c.NewMoney);
        }

        public async Task<ListDataInfo<ListPendingTempResponse>> ListPendingBuysAsync(SearchInfo pagination)
        {
            pagination.PageSize = 1000;
            var query = (from OF in _context.Offers
                         from US in _context.Users.Where(c => c.Id == OF.UserId).DefaultIfEmpty()
                         from UP in _context.Payers.Where(c => c.Id == OF.PayerId).DefaultIfEmpty()
                         from ID in _context.InvoiceDispersions.Where(c => c.OfferNumber == OF.Consecutive).DefaultIfEmpty()
                         where OF.StatusId == CatalogCode_OfferStatus.Enabled && OF.Status == true
                              && _context.InvoiceDispersions.Count(c => c.OfferNumber == OF.Consecutive) <= 0
                         group new { OfferNumber = OF.Consecutive, Saller = US.Company, Payer = UP.Name } by OF.Consecutive into gu
                         select new ListPendingTempResponse
                         {
                             Offer = gu.FirstOrDefault().OfferNumber,
                             NameSaller = gu.FirstOrDefault().Saller,
                             NamePayer = gu.FirstOrDefault().Payer,
                             Status = "Habilitada",
                             PurchasePercentage = 0,
                             ToolTip = "",
                             TxPurchased = "NA/NA",
                             OperationDate = null,
                             ExpectedDate = null,
                             EndDate = null,
                             Days = 0,
                         }).Union(from ID in _context.InvoiceDispersions
                                  from US in _context.Users.Where(c => c.Id == ID.SellerId).DefaultIfEmpty()
                                  from UP in _context.Payers.Where(c => c.Id == ID.PayerId).DefaultIfEmpty()
                                  where ID.Status == true
                                  group new { ID.OfferNumber, Saller = US.Company, Payer = UP.Name, ID.CurrentValue, ID.OperationDate, ID.ExpectedDate, ID.EndDate, ID.TransactionNumber } by ID.OfferNumber into g
                                  select new ListPendingTempResponse
                                  {
                                      Offer = g.FirstOrDefault().OfferNumber,
                                      NameSaller = g.FirstOrDefault().Saller,
                                      NamePayer = g.FirstOrDefault().Payer,
                                      Status = "",
                                      PurchasePercentage = Convert.ToInt32((_context.InvoiceDispersions
                                                                     .Where(c => c.OfferNumber == g.FirstOrDefault().OfferNumber && c.Status == true && c.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased)
                                                                     .Sum(s => s.CurrentValue) * 100) / g.Sum(s => s.CurrentValue)),
                                      ToolTip = Convert.ToString(_context.InvoiceDispersions
                                                                     .Where(c => c.OfferNumber == g.FirstOrDefault().OfferNumber && c.Status == true && c.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased)
                                                                     .Sum(s => s.CurrentValue)) + "/" + Convert.ToString(g.Sum(s => s.CurrentValue)),
                                      TxPurchased = _context.InvoiceDispersions.Count(c => c.OfferNumber == g.FirstOrDefault().OfferNumber && c.Status == true && c.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased).ToString() + "/" + g.Select(x => x.TransactionNumber).Distinct().Count().ToString(),
                                      OperationDate = g.FirstOrDefault().OperationDate,
                                      ExpectedDate = g.FirstOrDefault().ExpectedDate,
                                      EndDate = g.FirstOrDefault().EndDate,
                                      Days = 0
                                  });

            var result = new ListDataInfo<ListPendingTempResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.ToListAsync()
            };

            return result;
        }

        public async Task<ListDataInfo<ListPurchasedResponse>> ListPurchasedAsync(SearchInfo pagination)
        {
            if (pagination.ColumnOrder == "operationDate")
            {
                pagination.ColumnOrder = "OperationDateOrder";
            }

            var query = (from ID in _context.InvoiceDispersions
                         join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                         from US in _context.Users.Where(c => c.Id == ID.SellerId).DefaultIfEmpty()
                         from UP in _context.Payers.Where(c => c.Id == ID.PayerId).DefaultIfEmpty()
                         from EN in _context.EventNotifications.Where(c => c.OfferId == O.Id && c.TypeId == CatalogCode_TypeNotification.EmailSummaryOffer).DefaultIfEmpty()
                         where
                         ((string.IsNullOrEmpty(pagination.filter) || ID.OfferNumber.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || US.Company.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || ID.CurrentValue.ToString().Contains(pagination.filter)) ||
                          (string.IsNullOrEmpty(pagination.filter) || ID.FutureValue.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || UP.Name.ToLower().Contains(pagination.filter.ToLower())))
                         && _context.InvoiceDispersions.Count(c => c.OfferNumber == ID.OfferNumber && c.Status == true) ==
                         _context.InvoiceDispersions.Count(c => c.OfferNumber == ID.OfferNumber && c.Status == true && c.StatusId == CatalogCode_InvoiceDispersionStatus.Purchased)
                         && ID.Status == true
                         group new { ID.OfferNumber, O.Id, Saller = US.Company, Payer = UP.Name, ID.OperationDate, ID.CurrentValue, ID.FutureValue, EN.To } by ID.OfferNumber into g
                         select new ListPurchasedResponse
                         {
                             OfferId = g.FirstOrDefault().Id,
                             Offer = g.FirstOrDefault().OfferNumber,
                             NameSaller = g.FirstOrDefault().Saller,
                             NamePayer = g.FirstOrDefault().Payer,
                             OperationDateOrder = g.FirstOrDefault().OperationDate,
                             OperationDate = g.FirstOrDefault().OperationDate.DateddMMMyyyy(),
                             CurrentValue = g.Sum(s => s.CurrentValue),
                             FutureValue = g.Sum(s => s.FutureValue),
                             SummarySent = !string.IsNullOrEmpty(g.FirstOrDefault().To)
                         });

            var result = new ListDataInfo<ListPurchasedResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.Paginate(pagination).ToListAsync()
            };

            return result;
        }

        public async Task<HeaderDetailResponse> GetHeaderDetailAsync(int offerId)
        {
            var query = await (from ID in _context.InvoiceDispersions
                               join S in _context.Users on ID.SellerId equals S.Id
                               join P in _context.Payers on ID.PayerId equals P.Id
                               join o in _context.Offers on ID.OfferNumber equals o.Consecutive
                               from CS in _context.CatalogItems.Where(c => c.Id == ID.StatusId).DefaultIfEmpty()
                               where ID.OfferNumber == offerId && ID.Status == true
                               group new { ID.OfferNumber, Seller = S.Company, Payer = P.Name, ID.FutureValue, o.StatusId, CS.Name } by ID.OfferNumber into g
                               select new HeaderDetailResponse
                               {
                                   NroOffer = g.FirstOrDefault().OfferNumber,
                                   PayerName = g.FirstOrDefault().Payer,
                                   SellerName = g.FirstOrDefault().Seller,
                                   FutureValue = g.Sum(s => s.FutureValue),
                                   StatusId = g.FirstOrDefault().StatusId,
                                   Status = g.FirstOrDefault().Name
                               }).FirstOrDefaultAsync();

            return query;
        }

        public async Task<ListDataInfo<ListDetailResponse>> ListDetailAsync(int offerId, SearchInfo pagination)
        {
            var query = (from ID in _context.InvoiceDispersions
                         join O in _context.Offers on ID.OfferNumber equals O.Consecutive
                         from UB in _context.Users.Where(c => c.Id == ID.BuyerId).DefaultIfEmpty()
                         from S in _context.CatalogItems.Where(c => c.Id == ID.StatusId).DefaultIfEmpty()
                         where
                         (
                         (string.IsNullOrEmpty(pagination.filter) || ID.CurrentValue.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || ID.FutureValue.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || ID.OfferNumber.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || ID.TransactionNumber.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || UB.Company.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || S.Name.ToLower().Contains(pagination.filter.ToLower()))
                         )
                         && ID.OfferNumber == offerId && ID.Status == true
                         group new { ID.TransactionNumber, UB.Company, ID.StatusId, ID.FutureValue, S.Name, ID.CurrentValue, ID.Rate, ID.ExpirationDate } by ID.TransactionNumber into g
                         select new ListDetailResponse
                         {
                             NroOffer = offerId,
                             TrxId = g.FirstOrDefault().TransactionNumber,
                             NameBuyer = g.FirstOrDefault().Company,
                             StatusID = g.FirstOrDefault().StatusId,
                             Status = g.FirstOrDefault().Name,
                             CurrentValue = g.Sum(s => s.CurrentValue),
                             FutureValue = g.Sum(s => s.FutureValue),
                             RateAux = g.FirstOrDefault().Rate,
                             ExpirationDate = g.FirstOrDefault().ExpirationDate,
                         });

            ListDataInfo<ListDetailResponse> result = new();

            if (pagination.ColumnOrder == "timeLeft")
            {
                result = new ListDataInfo<ListDetailResponse>
                {
                    Count = await query.CountAsync(),
                    Data = await query.ToListAsync()
                };
            }
            else
            {
                result = new ListDataInfo<ListDetailResponse>
                {
                    Count = await query.CountAsync(),
                    Data = await query.Paginate(pagination).ToListAsync()
                };
            }

            return result;
        }

        public async Task<string> GetNameSellerByOfferAsync(int offerId)
        {
            var nameSeller = await (from U in _context.Users
                                    join ID in _context.InvoiceDispersions on U.Id equals ID.SellerId
                                    where ID.OfferNumber == offerId && ID.Status == true
                                    select U.Name).FirstOrDefaultAsync();
            return nameSeller;
        }

        public async Task<string> GetNameCompanySellerByOfferAsync(int offerId)
        {
            var nameCompanySeller = await (from U in _context.Users
                                           join ID in _context.InvoiceDispersions on U.Id equals ID.SellerId
                                           where ID.OfferNumber == offerId && ID.Status == true
                                           select U.Company).FirstOrDefaultAsync();
            return nameCompanySeller;
        }

        public async Task<List<Guid>> GetIdsAsync(int offerId, Guid payerId)
        {
            var invoiceDispersionId = await (from ID in _context.InvoiceDispersions
                                             where ID.OfferNumber == offerId && ID.PayerId == payerId && ID.Status == true
                                             && ID.Reallocation == 'I' && ID.CreatedOn >= ExtensionFormat.DateTimeCO().AddMinutes(-2)
                                             select ID.Id).ToListAsync();
            return invoiceDispersionId;
        }
    }
}