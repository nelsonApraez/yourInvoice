///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Invoices.Queries;
using System.Globalization;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISystem system;
        private const string eventDisable = "eventDisable";
        private const string eventSuccess = "eventSuccess";
        private const string eventReject = "eventReject";

        public InvoiceRepository(ApplicationDbContext context, ISystem system) : base(context, system)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.system = system;
        }

        public Invoice Add(Invoice invoice)
        {
            return base.Create(invoice);
        }

        public async Task<List<Invoice>> FindByStatus(Guid offerId, Guid statusId)
        {
            return await _context.Invoices.Where(x => x.OfferId == offerId && x.StatusId == statusId).ToListAsync();
        }

        public async Task<List<Invoice>> GetById(Guid id)
        {
            return await _context.Invoices.Where(x => x.Id == id).ToListAsync();
        }

        public async Task<List<Invoice>> FindByOfferId(Guid offerId)
        {
            return await _context.Invoices.Where(x => x.OfferId == offerId).ToListAsync();
        }

        public async Task DeleteAsync(List<Guid> invoiceIds)
        {
            await _context.Invoices.Where(x => invoiceIds.Contains(x.Id)).ExecuteDeleteAsync();
        }

        public async Task NullyfyAsync(Guid offerId)
        {
            await _context.Invoices.Where(x => x.OfferId == offerId).ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsByCufeWithStatusAsync(string cufe) => await _context.Invoices.AnyAsync(
            invoice => invoice.Cufe == cufe && (invoice.StatusId == CatalogCode_InvoiceStatus.InProgress || invoice.StatusId == CatalogCode_InvoiceStatus.Approved
            || invoice.StatusId == CatalogCode_InvoiceStatus.Loaded || invoice.StatusId == CatalogCode_InvoiceStatus.ValidationDian)
            && invoice.Status == true);

        public async Task<IEnumerable<Invoice>> GetAllByOffer(Guid offerId)
        {
            var result = await _context.Invoices.Where(c => c.OfferId == offerId).ToListAsync();
            return result;
        }

        public async Task<ListDataInfo<InvoiceListResponse>> ListAsync(Guid offerId, SearchInfo pagination)
        {
            if (pagination.ColumnOrder == "dueDate")
            {
                pagination.ColumnOrder = "DueDateOrder";
            }

            var query = (from u in _context.Invoices
                         from cs in _context.CatalogItems.Where(x => x.Id == u.StatusId).DefaultIfEmpty()
                         from cm in _context.CatalogItems.Where(x => x.Id == u.MoneyTypeId).DefaultIfEmpty()
                         where
                         ((string.IsNullOrEmpty(pagination.filter) || u.ZipName.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || u.Total.Value.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || cs.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || cm.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || u.Number.Contains(pagination.filter))) &&
                         u.OfferId == offerId &&
                         u.Status == true
                         select new InvoiceListResponse
                         {
                             Id = u.Id,
                             DueDate = u.DueDate.DateddMMMyyyy(),
                             DueDateOrder = u.DueDate,
                             Currency = cm.Name == ConstantCode_MoneyType.USD ? cm.Name + " TRM " + u.Trm.ToString() : cm.Name,
                             Status = cs.Name,
                             StatusId = cs.Id,
                             Value = u.Total.Value,
                             ZipName = u.ZipName,
                             InvoiceNumber = u.Number
                         });

            var result = new ListDataInfo<InvoiceListResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.Paginate(pagination).ToListAsync()
            };

            return result;
        }

        public async Task<ListDataInfo<InvoiceListEventsResponse>> ListEventsAsync(Guid offerId, SearchInfo pagination)
        {
            if (pagination.ColumnOrder == "dueDate")
            {
                pagination.ColumnOrder = "DueDateOrder";
            }
            if (pagination.ColumnOrder == "createdOn")
            {
                pagination.ColumnOrder = "CreatedOnOrder";
            }

            var query = (from u in _context.Invoices
                         from cs in _context.CatalogItems.Where(x => x.Id == u.StatusId).DefaultIfEmpty()
                         from cm in _context.CatalogItems.Where(x => x.Id == u.MoneyTypeId).DefaultIfEmpty()
                         from mr in _context.InvoiceEvents.Where(x => x.InvoiceId == u.Id).DefaultIfEmpty()
                         from mi in _context.CatalogItems.Where(x => x.Name.Trim() == u.ErrorMessage.Trim()).DefaultIfEmpty()
                         where
                         (
                         (string.IsNullOrEmpty(pagination.filter) || u.ZipName.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || u.Total.Value.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || cm.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || cs.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || u.Number.Contains(pagination.filter))
                         ) &&
                         u.OfferId == offerId &&
                         u.Status == true
                         select new InvoiceListEventsResponse
                         {
                             Id = u.Id,
                             DueDate = u.DueDate.DateddMMMyyyy(),
                             DueDateOrder = u.DueDate,
                             Status = cs.Name,
                             StatusId = cs.Id,
                             Value = u.Total.Value,
                             ZipName = u.ZipName,
                             InvoiceNumber = u.Number,
                             CreatedOnOrder = u.CreatedOn,
                             CreatedOn = u.CreatedOn.DateddMMMyyyy(),
                             Currency = cm.Name == ConstantCode_MoneyType.USD ? cm.Name + " TRM " + u.Trm.ToString() : cm.Name,
                             Events = SetEventsArray(_context.InvoiceEvents.Where(x => x.InvoiceId == u.Id).FirstOrDefault()),
                             RejectDescription = string.IsNullOrEmpty(mr.Message) ? mi.Descripton ?? string.Empty : mr.Message,
                         });
            var existsInvoiceAproved = await _context.Invoices.AnyAsync(c => c.Status == true && c.OfferId == offerId && c.StatusId == CatalogCode_InvoiceStatus.Approved);
            var existsInvoiceRejected = await _context.Invoices.AnyAsync(c => c.Status == true && c.OfferId == offerId && c.StatusId == CatalogCode_InvoiceStatus.Rejected);

            var result = new ListDataInfo<InvoiceListEventsResponse>
            {
                ExistsInvoiceAproved = existsInvoiceAproved,
                ExistsInvoiceRejected = existsInvoiceRejected,
                Count = await query.CountAsync(),
                Data = await query.Paginate(pagination).ToListAsync()
            };
            return result;
        }

        private static string[] SetEventsArray(InvoiceEvent dataEvent)
        {
            string[] eventColor = new string[7];
            if (dataEvent is not null)
            {
                eventColor[0] = GetColorEvent(dataEvent.Event030);
                eventColor[1] = GetColorEvent(dataEvent.Event032);
                eventColor[2] = GetColorEvent(dataEvent.Event033, IsRadian: false, isClaim: dataEvent.Claim ?? false);
                eventColor[3] = GetColorEvent(dataEvent.Event036, IsRadian: false, isEvent037_047: dataEvent.Event037 ?? false);
                eventColor[4] = GetColorEvent(dataEvent.Event037, IsRadian: true);
                eventColor[5] = GetColorEvent(dataEvent.Event06, IsRadian: true);
                eventColor[6] = GetColorEvent(dataEvent.Event07, IsRadian: true);
                return eventColor;
            }
            eventColor = new string[] { eventDisable, eventDisable, eventDisable, eventDisable, eventDisable, eventDisable, eventDisable };
            return eventColor;
        }

        private static string GetColorEvent(bool? dataEvent, bool IsRadian = false, bool isClaim = false, bool isEvent037_047 = false)
        {
            if (isEvent037_047)
            {
                return eventReject;
            }
            if (!isClaim && (dataEvent is null || !Convert.ToBoolean(dataEvent)))
            {
                return eventDisable;
            }
            return IsRadian || isClaim ? eventReject : eventSuccess;
        }

        public async Task<ListDataInfo<InvoiceListConfirmDataResponse>> ListConfirmAsync(Guid offerId, SearchInfo pagination)
        {
            if (pagination.ColumnOrder == "dueDate")
            {
                pagination.ColumnOrder = "DueDateOrder";
            }
            if (pagination.ColumnOrder == "negotiationDate")
            {
                pagination.ColumnOrder = "NegotiationDateOrder";
            }

            var query = from I in _context.Invoices
                        from CI in _context.CatalogItems.Where(x => x.Id == I.StatusId).DefaultIfEmpty()
                        join OF in _context.Offers on I.OfferId equals OF.Id
                        where
                         (
                         (string.IsNullOrEmpty(pagination.filter) || I.Number.Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || OF.Consecutive.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || CI.Name.ToLower().Contains(pagination.filter.ToLower())) ||
                         (string.IsNullOrEmpty(pagination.filter) || I.Total.Value.ToString().Contains(pagination.filter)) ||
                         (string.IsNullOrEmpty(pagination.filter) || I.NegotiationTotal.Value.ToString().Contains(pagination.filter))
                         )
                        && I.OfferId == offerId && I.Status == true
                        && (I.StatusId == CatalogCode_InvoiceStatus.Approved)
                        select new InvoiceListConfirmDataResponse
                        {
                            Id = I.Id,
                            Nro = 0,
                            ConsecutiveOffer = OF.Consecutive,
                            InvoiceNumber = I.Number,
                            DueDate = I.DueDate.DateddMMMyyyy(),
                            DueDateOrder = I.DueDate,
                            Status = CI.Name,
                            StatusId = CI.Id,
                            Value = I.Total.Value,
                            ValueIva = I.TaxAmount.Value,
                            NegotiationTotal = I.NegotiationTotal.Value,
                            NegotiationDate = I.NegotiationDate.DateddMMMyyyy(),
                            NegotiationDateOrder = I.NegotiationDate,
                        };

            var resultInvoices = new ListDataInfo<InvoiceListConfirmDataResponse>
            {
                Count = await query.CountAsync(),
                Data = await query.Paginate(pagination).ToListAsync()
            };
            return resultInvoices;
        }

        public async Task<bool> SaveChangeConfirmAsync(Guid offerId, List<Invoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                await _context.Invoices.Where(c => c.OfferId == offerId && c.Number.Trim() == invoice.Number.Trim())
                                                  .ExecuteUpdateAsync(p => p
                                                  .SetProperty(u => u.NegotiationTotal, invoice.NegotiationTotal)
                                                  .SetProperty(u => u.NegotiationDate, invoice.NegotiationDate)
                                                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO()));
            }
            return true;
        }

        public List<InvoiceListAppendix1DocumentResponse> ListToAppendix1Document(Guid offerId)
        {
            var gobal = CultureInfo.CreateSpecificCulture("es-ES");

            var query = (from u in _context.Invoices
                         from o in _context.Offers.Where(x => x.Id == u.OfferId)
                         from s in _context.Users.Where(x => x.Id == o.UserId).DefaultIfEmpty()
                         from p in _context.Payers.Where(x => x.Id == o.PayerId).DefaultIfEmpty()
                         where
                         u.OfferId == offerId &&
                         u.Status == true &&
                         u.StatusId != CatalogCode_InvoiceStatus.Rejected
                         select new InvoiceListAppendix1DocumentResponse
                         {
                             InvoiceNumber = u.Number,
                             Cufe = u.Cufe,
                             DueDate = u.DueDate.HasValue ? u.DueDate.Value.ToString("dd/MM/yyyy", gobal) : "sin fecha",
                             EmitDate = u.EmitDate.HasValue ? u.EmitDate.Value.ToString("dd/MM/yyyy", gobal) : "sin fecha",
                             NegotiationDate = u.NegotiationDate.HasValue ? u.NegotiationDate.Value.ToString("dd/MM/yyyy", gobal) : "sin fecha",
                             NegotiationTotal = u.NegotiationTotal.HasValue ? (decimal)u.NegotiationTotal : 0,
                             PayerName = p.Name,
                             SellerName = s.Company,
                             NitPayer = p.Nit
                         });

            return query.ToList();
        }

        public List<InvoiceListGenerateExcelResponse> ListToGenerateExcel(Guid offerId)
        {
            var gobal = CultureInfo.CreateSpecificCulture("es-ES");

            var query = (from u in _context.Invoices
                         from o in _context.Offers.Where(x => x.Id == u.OfferId)
                         from s in _context.Users.Where(x => x.Id == o.UserId).DefaultIfEmpty()
                         from p in _context.Payers.Where(x => x.Id == o.PayerId).DefaultIfEmpty()
                         where
                         u.OfferId == offerId &&
                         u.Status == true && (u.StatusId == CatalogCode_InvoiceStatus.Approved)
                         select new InvoiceListGenerateExcelResponse
                         {
                             DocumentType = "Factura",
                             OfferConsecutive = o.Consecutive,
                             InvoiceNumber = u.Number,
                             PayerNit = p.Nit,
                             SellerNit = s.CompanyNit,
                             DueDate = u.DueDate.HasValue ? u.DueDate.Value.ToString("dd/MM/yyyy", gobal) : "sin fecha",
                             EmitDate = u.EmitDate.HasValue ? u.EmitDate.Value.ToString("dd/MM/yyyy", gobal) : "sin fecha",
                             NegotiationDate = u.NegotiationDate.HasValue ? u.NegotiationDate.Value.ToString("dd/MM/yyyy", gobal) : "sin fecha",
                             NegotiationTotal = u.NegotiationTotal.HasValue ? (decimal)u.NegotiationTotal : 0,
                             PayerName = p.Name,
                             SellerName = s.Company
                         });

            return query.ToList();
        }

        public async Task<InvoiceSumPayerSellerResponse> GetInvoiceSumPayerSellerAsync(Guid offerId)
        {
            var query = await (from O in _context.Offers
                               join I in _context.Invoices on O.Id equals I.OfferId
                               join P in _context.Payers on O.PayerId equals P.Id
                               join S in _context.Users on O.UserId equals S.Id
                               where O.Id == offerId
                               group new { Saller = S.Name, SallerCompany = S.Company, Payer = P.Name, I.Total, I.StatusId } by O.Consecutive into g
                               select new InvoiceSumPayerSellerResponse
                               {
                                   PayerName = g.FirstOrDefault().Payer,
                                   SellerName = g.FirstOrDefault().Saller,
                                   SellerCompany = g.FirstOrDefault().SallerCompany,
                                   InvoiceSum = g.Where(x => x.StatusId == CatalogCode_InvoiceStatus.Approved).Sum(s => s.Total),
                               }).FirstOrDefaultAsync();

            return query;
        }

        public async Task<bool> OfferIsInProgressAsync(Guid offerId)
        {
            return await _context.Offers.AnyAsync(x => x.Id == offerId && x.StatusId == CatalogCode_OfferStatus.InProgress);
        }
    }
}