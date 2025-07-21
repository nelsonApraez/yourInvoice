///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Accounts.Queries;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.Accounts.Reject
{
    public class RejectAccountCommandHandler : IRequestHandler<RejectAccountCommand, ErrorOr<bool>>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IUnitOfWorkLink unitOfWork;
        private readonly ICatalogBusiness _catalogBusiness;

        private const string Subject = "Usuario rechazado en yourInvoicedigital";

        public RejectAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWorkLink unitOfWork,
             ICatalogBusiness catalogBusiness)
        {
            this.accountRepository = accountRepository;
            this.unitOfWork = unitOfWork;
            _catalogBusiness = catalogBusiness;
        }

        public async Task<ErrorOr<bool>> Handle(RejectAccountCommand command, CancellationToken cancellationToken)
        {
            if (await accountRepository.GetByIdAsync(command.Id) is not AccountResponse account)
            {
                return Error.NotFound(MessageCodes.AccountNotExist, GetErrorDescription(MessageCodes.AccountNotExist));
            }

            bool result = await SaveInDB(account, cancellationToken);
            await SendEmail(account);

            return result;
        }

        private async Task SendEmail(Account account)
        {
            var templateUser = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToRejectPreRegister);

            Dictionary<string, string> replacements = new()
            {
                { "{{year}}", ExtensionFormat.DateTimeCO().Year.ToString() }
            };

            string templateUserWithData = TransformModule.ReplaceTokens(templateUser.Descripton, replacements);

            EmainBusiness emainBusiness = new(_catalogBusiness);

            //email para usuario
            await emainBusiness.SendAsync(account.Email, Subject, templateUserWithData);
        }

        private async Task<bool> SaveInDB(Account account, CancellationToken cancellationToken)
        {
            account.StatusId = CatalogCode_StatusPreRegister.Rejected;

            bool result = await accountRepository.UpdateAsync(account);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}