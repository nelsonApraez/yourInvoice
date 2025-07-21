///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Common;
using yourInvoice.Offer.Domain.Invoices.Queries;

namespace yourInvoice.Offer.Domain.Invoices
{
    public interface IInvoiceRepository : IRepositoryBase<Invoice>
    {
        Task<IEnumerable<Invoice>> GetAllByOffer(Guid offerId);

        Invoice Add(Invoice invoice);

        Task<bool> ExistsByCufeWithStatusAsync(string cufe);

        Task DeleteAsync(List<Guid> invoiceIds);

        Task NullyfyAsync(Guid offerId);

        Task<ListDataInfo<InvoiceListResponse>> ListAsync(Guid offerId, SearchInfo pagination);

        Task<ListDataInfo<InvoiceListEventsResponse>> ListEventsAsync(Guid offerId, SearchInfo pagination);

        List<InvoiceListAppendix1DocumentResponse> ListToAppendix1Document(Guid offerId);

        Task<List<Invoice>> FindByStatus(Guid offerId, Guid statusId);

        Task<ListDataInfo<InvoiceListConfirmDataResponse>> ListConfirmAsync(Guid offerId, SearchInfo pagination);

        Task<bool> SaveChangeConfirmAsync(Guid offerId, List<Invoice> invoices);

        Task<List<Invoice>> GetById(Guid id);

        Task<List<Invoice>> FindByOfferId(Guid offerId);

        List<InvoiceListGenerateExcelResponse> ListToGenerateExcel(Guid offerId);

        Task<InvoiceSumPayerSellerResponse> GetInvoiceSumPayerSellerAsync(Guid offerId);

        Task<bool> OfferIsInProgressAsync(Guid offerId);
    }
}