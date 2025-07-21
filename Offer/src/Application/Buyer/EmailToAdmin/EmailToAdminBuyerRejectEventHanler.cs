///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Offer.Application.Buyer.EmailToAdmin
{
    public class EmailToAdminBuyerRejectEventHanler : INotificationHandler<EmailToAdminBuyerRejectCommand>
    {
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IUserRepository userRepository;

        public EmailToAdminBuyerRejectEventHanler(ICatalogBusiness catalogBusiness, IUserRepository userRepository)
        {
            this.catalogBusiness = catalogBusiness;
            this.userRepository = userRepository;
        }

        public async Task Handle(EmailToAdminBuyerRejectCommand notification, CancellationToken cancellationToken)
        {
            var enlaceyourInvoice = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.UrlyourInvoice);
            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailBuyerRejectToAdmin);
            if (notification.AttachData is null)
            {
                notification.AttachData = new();
            }
            notification.AttachData.Add("{{NumeroOferta}}", notification.NumberOffer.ToString());
            notification.AttachData.Add("{{NumeroTransaccion}}", notification.TransactionNumber.ToString());
            notification.AttachData.Add("{{NombreComprador}}", notification.NameBuyer);
            notification.AttachData.Add("{{urlFileExcel}}", notification.UrlFileExcel);
            notification.AttachData.Add("{{urlyourInvoice}}", enlaceyourInvoice.Descripton);
            notification.AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, notification.AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            var emailAdmin = await this.userRepository.GetEmailRoleAsync(CatalogCode_UserRole.Administrator);
            await emainBusiness.SendAsync(emailAdmin, "Oferta " + notification.NumberOffer + " rechazada", templateAdminWithData);
        }
    }
}