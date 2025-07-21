///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Offer.Application.Buyer.EmailToAdmin
{
    public class EmailToAdminBuyerApproveEventHandler : INotificationHandler<EmailToAdminBuyerApproveCommand>
    {
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly IUserRepository userRepository;
        public EmailToAdminBuyerApproveEventHandler(ICatalogBusiness catalogBusiness, IInvoiceDispersionRepository invoiceDispersionRepository, IUserRepository userRepository)
        {
            this.catalogBusiness = catalogBusiness;
            this.invoiceDispersionRepository = invoiceDispersionRepository;
            this.userRepository = userRepository;
        }

        public async Task Handle(EmailToAdminBuyerApproveCommand notification, CancellationToken cancellationToken)
        {
            var approvedPercentage = await this.invoiceDispersionRepository.GetPurchasePercentageAsync(notification.NumberOffer);
            if (approvedPercentage < 100)
            {
                return;
            }
            var moneyOrderDocumentMissing = await this.invoiceDispersionRepository.ThereIsNoMissingMoneyTransferDocument(notification.NumberOffer);
            if (!moneyOrderDocumentMissing)
            {
                return;
            }
            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailBuyerApproveToAdmin);
            if (notification.AttachData is null)
            {
                notification.AttachData = new();
            }
            notification.AttachData.Add("{{NumeroOferta}}", notification.NumberOffer.ToString());
            notification.AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, notification.AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            var emailAdmin = await this.userRepository.GetEmailRoleAsync(CatalogCode_UserRole.Administrator);
            await emainBusiness.SendAsync(emailAdmin, "Oferta comprada", templateAdminWithData);
        }
    }
}