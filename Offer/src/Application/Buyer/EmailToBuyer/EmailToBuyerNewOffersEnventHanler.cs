///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;

namespace yourInvoice.Offer.Application.Buyer.EmailToBuyer
{
    public class EmailToBuyerNewOffersEnventHanler : INotificationHandler<EmailToBuyerNewOffersCommand>
    {
        private readonly ICatalogBusiness catalogBusiness;

        public EmailToBuyerNewOffersEnventHanler(ICatalogBusiness catalogBusiness)
        {
            this.catalogBusiness = catalogBusiness;
        }

        public async Task Handle(EmailToBuyerNewOffersCommand notification, CancellationToken cancellationToken)
        {
            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailBuyerNewOffer);
            var urlLogueo = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.UrlyourInvoice);
            if (notification.AttachData is null)
            {
                notification.AttachData = new();
            }
            notification.AttachData.Add("{{NumeroOferta}}", notification.NumberOffer.ToString());
            notification.AttachData.Add("{{UrlLogeoPlataformayourInvoice}}", urlLogueo?.Descripton ?? string.Empty);
            notification.AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, notification.AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            await emainBusiness.SendAsync(notification.EmailBuyerNotification, "Tienes una nueva oferta disponible", templateAdminWithData);
        }
    }
}