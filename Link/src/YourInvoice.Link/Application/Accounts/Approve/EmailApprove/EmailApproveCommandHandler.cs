///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.Extension;

namespace yourInvoice.Link.Application.Accounts.Approve.EmailApprove
{
    public class EmailApproveCommandHandler : INotificationHandler<EmailApproveCommand>
    {
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IUserRepository userRepository;

        public EmailApproveCommandHandler(ICatalogBusiness catalogBusiness, IUserRepository userRepository)
        {
            this.catalogBusiness = catalogBusiness;
            this.userRepository = userRepository;
        }

        public async Task Handle(EmailApproveCommand notification, CancellationToken cancellationToken)
        {
            var enlaceyourInvoice = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.UrlyourInvoice);
            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailApproveAccount);
            if (notification.AttachData is null)
            {
                notification.AttachData = new();
            }
            notification.AttachData.Add("{{urlyourInvoice}}", enlaceyourInvoice.Descripton);
            notification.AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, notification.AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            await emainBusiness.SendAsync(notification.EmailApprove.Trim(), "Habilitación de cuenta", templateAdminWithData);
        }
    }
}