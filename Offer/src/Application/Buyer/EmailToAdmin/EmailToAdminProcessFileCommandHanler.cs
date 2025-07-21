///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.Users;
using System.Text;

namespace yourInvoice.Offer.Application.Buyer.EmailToAdmin
{
    public class EmailToAdminProcessFileCommandHanler : INotificationHandler<EmailToAdminProcessFileCommand>
    {
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IUserRepository userRepository;

        public EmailToAdminProcessFileCommandHanler(ICatalogBusiness catalogBusiness, IUserRepository userRepository)
        {
            this.catalogBusiness = catalogBusiness;
            this.userRepository = userRepository;
        }

        public async Task Handle(EmailToAdminProcessFileCommand notification, CancellationToken cancellationToken)
        {
            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToAdminProcessFile);
            if (notification.AttachData is null)
            {
                notification.AttachData = new();
            }
            var messageValidationHtml = GetMessageValidationFileHtml(notification.MessageValidationFile);
            notification.AttachData.Add("{{NombreArchivo}}", notification.NameFile.Trim());
            notification.AttachData.Add("{{LinkArchivo}}", notification.LinkFile);
            notification.AttachData.Add("{{MensajeValidacion}}", messageValidationHtml);
            notification.AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, notification.AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            var emailAdmin = await this.userRepository.GetEmailRoleAsync(CatalogCode_UserRole.Administrator);
            await emainBusiness.SendAsync(emailAdmin, $"ERROR - Procesamiento de archivo {notification.NameFile.Trim()}", templateAdminWithData);
        }

        private string GetMessageValidationFileHtml(string messageValidation)
        {
            var messages = messageValidation.Split(".");
            messages = messages.Where(m => !string.IsNullOrEmpty(m.Trim())).ToArray();
            var htmlMessage = new StringBuilder();
            foreach (var message in messages)
            {
                htmlMessage.Append($"<li style=\"text-align: left\">{message}</li>");
            }
            return htmlMessage.ToString();
        }
    }
}