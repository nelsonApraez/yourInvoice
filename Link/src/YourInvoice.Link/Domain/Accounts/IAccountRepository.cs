///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Domain.Accounts.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;

namespace yourInvoice.Link.Domain.Accounts
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountAsync();

        Account Add(Account account);

        Task<bool> ExistsByEmailAsync(string email);

        Task<Account?> GetByEmailAsync(string email);

        Task<AccountResponse> GetByIdAsync(Guid Id);

        Task<ListDataInfo<ListResponse>?> GetListAsync(SearchInfo pagination);

        Task<ListDataInfo<ListLinkingProccessResponse>?> GetListLinkingProcessesAsync(SearchInfo pagination);

        Task<bool> ExistsByIdAsync(Guid id);

        Task<bool> UpdateAsync(Account account);

        Task<bool> UpdateStatusAsync(Guid accountId, Guid statusId, Guid userId, int timeHour);

        Task<AccountResponse> GetAccountIdAsync(Guid id);

        Task<LinkStatus?> GetStatusLinkAsync(Guid IdUserLink);
    }
}