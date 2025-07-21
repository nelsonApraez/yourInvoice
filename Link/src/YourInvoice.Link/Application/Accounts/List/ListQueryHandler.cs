///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Accounts.Queries;

namespace yourInvoice.Link.Application.Accounts.List
{
    public sealed class ListQueryHandler : IRequestHandler<ListQuery, ErrorOr<ListDataInfo<ListResponse>>>
    {
        private readonly IAccountRepository accountRepository;
        private readonly string orderDefault = "statusid";
        private readonly string fieldOrder="name";
        private readonly string fieldOrderChange = "NameOrder";

        public ListQueryHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<ErrorOr<ListDataInfo<ListResponse>>> Handle(ListQuery query, CancellationToken cancellationToken)
        {
            query.pagination.ColumnOrder = query.pagination.ColumnOrder.ToLowerInvariant().Equals(fieldOrder) ? fieldOrderChange : query.pagination.ColumnOrder;
            var account = await this.accountRepository.GetListAsync(query.pagination);
            if (account is not null && account.Data?.Count > 0 && query.pagination.ColumnOrder.ToLowerInvariant().Equals(orderDefault))
            {
                var data = account.Data.OrderBy(o => o.OrderRegister).ThenByDescending(o => o.CreatedOn);
                return new ListDataInfo<ListResponse>
                {
                    Count = account.Count,
                    Data = data.ToList(),
                };
            }
            if (account is not null && query.pagination.ColumnOrder == "time")
            {
                var IsOrderAsc = query.pagination.OrderType.ToLowerInvariant().Equals("asc");
                var DataTemp = IsOrderAsc ? account.Data.OrderBy(o => o.Time).ToList() : account.Data.OrderByDescending(o => o.Time).ToList();
                var result = new ListDataInfo<ListResponse>
                {
                    Count = DataTemp.Count,
                    Data = DataTemp.Skip(query.pagination.StartIndex).Take(query.pagination.PageSize).ToList()
                };
                
                return result;
            }

            return account ?? new ListDataInfo<ListResponse>();
        }
    }
}