///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Offer.Application.Admin.EmailToSeller
{
    public class EmailToSellerAdminPurchasedCommandHanler : INotificationHandler<EmailToSellerAdminPurchasedCommand>
    {
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IEventNotificationRepository eventNotificationRepository;
        private readonly Domain.Offers.IOfferRepository offerRepository;
        private readonly IUnitOfWorkCommonEF unitOfWork;

        public EmailToSellerAdminPurchasedCommandHanler(ICatalogBusiness catalogBusiness, IEventNotificationRepository eventNotificationRepository, Domain.Offers.IOfferRepository offerRepository, IUnitOfWorkCommonEF unitOfWork)
        {
            this.catalogBusiness = catalogBusiness;
            this.eventNotificationRepository = eventNotificationRepository;
            this.offerRepository = offerRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(EmailToSellerAdminPurchasedCommand notification, CancellationToken cancellationToken)
        {
            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToSellerAdminPurchased);
            if (notification.AttachData is null)
            {
                notification.AttachData = new();
            }
            notification.AttachData.Add("{{NumeroOferta}}", notification.NumberOffer.ToString());
            notification.AttachData.Add("{{NombreVendedor}}", notification.NameSeller);
            notification.AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, notification.AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            Domain.Offer offer = await offerRepository.GetByConsecutiveAsync(notification.NumberOffer);
            foreach (var email in notification.EmailsSeller)
            {
                await emainBusiness.SendAsync(email, "La oferta (" + notification.NumberOffer + ") ha sido comprada", templateAdminWithData, attachFile: notification.AttachFilesData);
                await SaveEventNotificationAsync(CatalogCode_TypeNotification.EmailSummaryOffer, offer.Id, email, templateAdminWithData);
            }
        }

        private async Task<bool> SaveEventNotificationAsync(Guid typeEnvent, Guid offerId, string to, string body)
        {
            var dataEvent = new EventNotificationInfo { Id = Guid.NewGuid(), TypeId = typeEnvent, OfferId = offerId, To = to, Body = body };
            var result = await this.eventNotificationRepository.AddEventNotificationAsync(dataEvent);
            await this.unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}