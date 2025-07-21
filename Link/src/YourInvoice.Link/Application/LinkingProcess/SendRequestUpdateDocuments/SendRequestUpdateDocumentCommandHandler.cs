///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.LinkingProcess.SendRequestUpdateDocuments
{
    using yourInvoice.Common.Business.CatalogModule;
    using yourInvoice.Link.Application.LinkingProcess.EmailToRequestDocument;
    using yourInvoice.Link.Domain.Accounts;

    public class SendRequestUpdateDocumentCommandHandler : IRequestHandler<SendRequestUpdateDocumentCommand, ErrorOr<bool>>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMediator mediator;

        public SendRequestUpdateDocumentCommandHandler(IAccountRepository accountRepository, IMediator mediator)
        {
            this.accountRepository = accountRepository;
            this.mediator = mediator;
        }

        public async Task<ErrorOr<bool>> Handle(SendRequestUpdateDocumentCommand command, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccountIdAsync(command.accountId);

            if (account is null || account?.Email?.Length <= 0)
            {
                return false;
            }

            string displayLabel = string.Empty;

            if (account?.PersonTypeId == CatalogCode_PersonType.Natural)
            {
                displayLabel = "Señor(a)";
            }
            else if (account?.PersonTypeId == CatalogCode_PersonType.Juridica)
            {
                displayLabel = "Señores";
            }

            var emailToRequest = new EmailToRequestDocumentCommand
            {
                Label = displayLabel,
                Name = command.request.DisplayName,
                Message = command.request.Message,
                Email = command.request.Email ?? string.Empty,
            };

            await this.mediator.Publish(emailToRequest, cancellationToken);

            return true;
        }
    }
}
