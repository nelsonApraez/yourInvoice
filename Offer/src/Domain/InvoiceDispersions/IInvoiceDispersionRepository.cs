///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Admin.Queries;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;
using yourInvoice.Offer.Domain.Offers.Queries;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Offer.Domain.InvoiceDispersions
{
    public interface IInvoiceDispersionRepository
    {
        Task<bool> AddAsync(IEnumerable<InvoiceDispersion> invoicesDispersion);

        Task<bool> UpdateAsync(List<InvoiceDispersion> invoiceDispersion);

        Task<IEnumerable<InvoiceDispersion>> GetAllDefeatedAsync(DateTime expirationDate, Guid statusRejected);

        Task<IEnumerable<InvoiceDispersion>> GetFilesWithoutProcessAsync();

        Task<DataValidationProcessFileResponse> ValidateExistsAsync(int offerNumber, int noTransaction, string documentNumberBuyer);

        Task<bool> UpdateReassignmentAsync(List<InvoiceDispersion> invoiceDispersions);

        Task<ResumeResponse> GetResumeAsync(int numberOffer, Guid userId = default);

        Task<ListDataInfo<ResumeInvoiceResponse>> ResumeInvoiceAsync(SearchInfo pagination, int numberOffer, Guid userId = default);

        Task<List<ResumeInvoiceExelResponse>> ResumeInvoiceExelAsync(int numberOffer, Guid buyerId, Guid statusId);

        Task<bool> ChangeStatusToRejectInvoiceAsync(int numberOffer, Guid buyerId, Guid statusId);

        Task<int> GetPurchasePercentageAsync(int numberOffer, Guid userId = default);

        Task<bool> ThereIsNoMissingMoneyTransferDocument(int numberOffer, Guid userId = default);

        Task<Guid> ValidateExistsSellerAsync(int numberOffer, string documentNumberSeller);

        Task<List<OfferListResponse>> ListToPurchaseCertificateDocument(Guid buyerId, int offerNumber);

        long GetTotalPurchased(Guid buyerId, int offerNumber);

        bool IsNotNewMoneyOrMixed(Guid buyerId, int offerNumber);

        Task<InvoiceDispersion> GetByOfferNumberAndBuyerIdAsync(int offerNumber, Guid buyerId);

        Task<List<InvoiceDispersion>> FindByOfferNumberAndBuyerIdAsync(int offerNumber, Guid buyerId);

        Task<HeaderTransactionResponse> GetHeaderTransactionAsync(int transactionId);

        Task<List<ListTransactionsResponse>> ListTransactionsAsync(int transactionId);

        Task<InvoiceDispersion> GetByTransactionNumberAsync(int transactionNumber);

        Task<HeaderTransactionResponse> GetHeaderOfferAsync(int offerId);

        Task<ListDataInfo<ListPendingTempResponse>> ListPendingBuysAsync(SearchInfo pagination);

        Task<ListDataInfo<ListPurchasedResponse>> ListPurchasedAsync(SearchInfo pagination);

        Task<HeaderDetailResponse> GetHeaderDetailAsync(int offerId);

        Task<ListDataInfo<ListDetailResponse>> ListDetailAsync(int offerId, SearchInfo pagination);

        Task<string> GetNameSellerByOfferAsync(int offerId);

        Task<List<Guid>> GetIdsAsync(int offerId, Guid payerId);

        Task ChangeStatusInvoiceDispersionPurchasedAsync(List<InvoiceDispersion> invoiceDispersions);

        Task<string> GetNameCompanySellerByOfferAsync(int offerId);
        Task<int> CountTransactionAsync(int offerNumber, int noTransaction, string documentNumberBuyer);
        Task<DataValidationProcessFileResponse> ValidateExistsInvoiceAsync(int offerNumber, string facturaNo, string documentNumberBuyer);
    }
}