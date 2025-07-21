///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;

namespace yourInvoice.Link.Application.LinkingProcess.EmailToAdminLinkingCompletedLegal
{
    public class EmailLinkingCompletedLegalCommandHandler : INotificationHandler<EmailLinkingCompletedLegalCommand>
    {
        private readonly IAccountRepository accountRepository;
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IUserRepository userRepository;
        public Dictionary<string, string> AttachData { get; set; }

        public EmailLinkingCompletedLegalCommandHandler(ICatalogBusiness catalogBusiness, IAccountRepository accountRepository, IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
            this.catalogBusiness = catalogBusiness;
        }

        public async Task Handle(EmailLinkingCompletedLegalCommand command, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccountIdAsync(command.accountId);

            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToAdminLinkingCompletedPJ);
            if (AttachData is null)
            {
                AttachData = new();
            }

            var urlVinculacionLegal = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.urlVinculacionCompletedEmailToAdmin);

            var asunto = "Se ha recibido una nueva solicitud de vinculación - " + account.SocialReason;

            AttachData.Add("{{RazonSocial}}", account?.SocialReason ?? string.Empty);
            AttachData.Add("{{Nit}}", $"{account?.Nit ?? string.Empty}-{account?.DigitVerify ?? string.Empty}");
            AttachData.Add("{{urlVinculacionLegal}}", $"{urlVinculacionLegal.Descripton}/{command.accountId}"); ;
            AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            var emailAdmin = await this.userRepository.GetEmailRoleAsync(CatalogCode_UserRole.Administrator);
            await emainBusiness.SendAsync(emailAdmin, asunto, templateAdminWithData);

        }
    }

}