///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Offers.Queries;

namespace yourInvoice.Offer.Domain.Offers
{
    public interface IOfferRepository
    {
        Task<List<Offer>> GetAll();

        Task<Offer> GetByIdAsync(Guid id);

        Task<bool> ExistsAsync(Guid id);

        Offer Add(Offer offer);

        void Update(Offer offer);

        Task DeleteAsync(Guid offerId);

        Task<Offer> GetOfferAsync(Guid offerId);

        Task<GetOfferResponse> GetByIdWithNamesAsync(Guid offerId);

        Task<IReadOnlyList<Invoice>> GetInvoiceProcessConfirmAsync(Guid offerId);

        Task<ListDataInfo<ListAllOfferResponse>> ListAllByUserAsync(SearchInfo pagination, Guid userId = default);

        Task<DetailOfferResponse> DetailAsync(Guid offerId, Guid userId = default);

        Task<Domain.Offer> GetByConsecutiveAsync(int consecutive);

        Task<ProcessFileValidationResponse> GetDataValidateStateOfferEnabledAsync(int offerNumber, string nroInvoice, string documentPayer);

        Task<Guid> GetIdOfferAsync(int offerNumber);

        Task<bool> OfferIsInProgressAsync(Guid id);

        Task<bool> OfferIsInProgressByInvoiceIdAsync(Guid invoiceId);

        Task<bool> OfferIsInProgressByBeneficiaryIdAsync(Guid beneficiaryId);

        Task<bool> OfferIsPurchasedAsync(int consecutive);

        Task<decimal?> TotalOfferAsync(Guid offerId);
    }
}