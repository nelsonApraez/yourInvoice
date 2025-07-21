///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.LinkingProcess.EmailToRequestDocument
{
    using yourInvoice.Common.Business.CatalogModule;
    using yourInvoice.Common.Business.EmailModule;
    using yourInvoice.Common.Business.TransformModule;
    using yourInvoice.Common.Extension;

    public class EmailToRequestDocumentCommandHandler : INotificationHandler<EmailToRequestDocumentCommand>
    {
        private readonly ICatalogBusiness catalogBusiness;

        public EmailToRequestDocumentCommandHandler(ICatalogBusiness catalogBusiness)
        {
            this.catalogBusiness = catalogBusiness;
        }

        public async Task Handle(EmailToRequestDocumentCommand notification, CancellationToken cancellationToken)
        {
            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.MailRequestAdicionalDocument);

            if (notification.AttachData is null)
            {
                notification.AttachData = new();
            }

            notification.AttachData.Add("{{displayLabel}}", notification.Label);
            notification.AttachData.Add("{{displayName}}", notification.Name);
            notification.AttachData.Add("{{displayMessage}}", notification.Message);
            notification.AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());

            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, notification.AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            await emainBusiness.SendAsync(notification.Email.Trim(), "Solicitud de documentos adicionales - yourInvoice Digital", templateAdminWithData);
        }
    }
}
