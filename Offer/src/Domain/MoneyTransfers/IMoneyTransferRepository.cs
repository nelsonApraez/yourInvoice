///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.MoneyTransfers.Queries;

namespace yourInvoice.Offer.Domain.MoneyTransfers
{
    public interface IMoneyTransferRepository
    {
        MoneyTransfer Add(MoneyTransfer moneyTransfer);

        Task DeleteAsync(Guid moneyTransferId);

        Task<bool> ExistsByDocumentAsync(string document, Guid offerId, Guid bankId);

        Task<ListDataInfo<BeneficiariesListResponse>> ListAsync(Guid offerId, SearchInfo pagination);

        Task<MoneyTransfer> GetByIdAsync(Guid id);

        Task<List<MoneyTransfer>> GetAllByOfferId(Guid offerId);

        Task<bool> ExistsByIdAsync(Guid id);

        Task<MoneyTransferDocumentResponse> ListToMoneyTransferDocumentAsync(Guid offerId);

        Task<int> GetCountId(Guid offerId);

        Task<int> CountBeneficiaryAsync(string document, Guid offerId);

        Task<decimal?> TotalAsync(Guid offerId);
    }
}