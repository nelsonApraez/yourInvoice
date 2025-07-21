///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Link.Domain.Accounts.Queries;
using yourInvoice.Link.Domain.Accounts;

namespace yourInvoice.Link.Application.Accounts.GetAccount
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, ErrorOr<AccountResponse>>
    {
        private readonly IAccountRepository _repository;

        public GetAccountQueryHandler(IAccountRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<AccountResponse>> Handle(GetAccountQuery query, CancellationToken cancellationToken)
        {
            if (await _repository.GetByIdAsync(query.Id) is not AccountResponse accountResponse)
            {
                return Error.NotFound(MessageCodes.AccountNotExist, GetErrorDescription(MessageCodes.AccountNotExist));
            }

            return accountResponse;
        }
    }
}