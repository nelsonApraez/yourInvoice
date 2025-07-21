///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.IdentityModel.Tokens;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.AccountRoles;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.Application.LinkingProcess.ApproveLink.EmailApprove
{
    public class EmailApproveNotificationCommandHandler : INotificationHandler<EmailApproveNotificationCommand>
    {
        private readonly ISystem _system;
        private readonly ICatalogBusiness _catalogBusiness;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IGeneralInformationRepository _generalInformationRepository;
        private readonly ILegalGeneralInformationRepository _legalGeneralInformationRepository;

        public EmailApproveNotificationCommandHandler(ISystem system, ICatalogBusiness catalogBusiness, IAccountRepository accountRepository,
            IGeneralInformationRepository generalInformationRepository, ILegalGeneralInformationRepository legalGeneralInformationRepository)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));
            _catalogBusiness = catalogBusiness ?? throw new ArgumentNullException(nameof(catalogBusiness));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _generalInformationRepository = generalInformationRepository ?? throw new ArgumentNullException(nameof(generalInformationRepository));
            _legalGeneralInformationRepository = legalGeneralInformationRepository ?? throw new ArgumentNullException(nameof(legalGeneralInformationRepository));
        }

        public async Task Handle(EmailApproveNotificationCommand command, CancellationToken cancellationToken)
        {
            var user = _system.User.Email;
            var template = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.MailNotificationLinkApproval);
            var link = await _catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.UrlyourInvoice);

            var account = await _accountRepository.GetByIdAsync(command.Id);

            if (account == null)
                return;

            string header = "";

            if (account.PersonTypeId.Equals(CatalogCode_PersonType.Natural))
            {
                var data = await _generalInformationRepository.GetGeneralInformationIdAsync(command.Id);
                var name = ""
                    + data.FirstName
                    + (data.SecondName.IsNullOrEmpty() ? " " : $" {data.SecondName} ")
                    + data.LastName
                    + (data.SecondLastName.IsNullOrEmpty() ? " " : $" {data.SecondLastName}");

                header = $"Señor(a) {name} - {account.CustomerType}";
            }
            else
            {
                var data = await _legalGeneralInformationRepository.GetLegalGeneralInformationAsync(command.Id);

                header = $"Señores {data.CompanyName} - {account.CustomerType}";
            }

            var body = new Dictionary<string, string>
            {
                { "{{Header}}", header },
                { "{{urlyourInvoice}}", link.Descripton },
                { "{{year}}", ExtensionFormat.DateTimeCO().Year.ToString() }
            };

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, body);

            EmainBusiness emainBusiness = new(_catalogBusiness);
            await emainBusiness.SendWithCCAsync(account.Email.Trim(), user.Trim(), "Su solicitud de vinculación ha sido aprobada", templateWithData);
        }
    }
}
