///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.Notifications;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Buyer.Expired
{
    public sealed class ExpiredCommandHandler : IRequestHandler<ExpiredCommand, ErrorOr<bool>>
    {
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IUnitOfWork unitOfWork;
        private readonly IInvoiceDispersionRepository invoiceDispersionRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly ISystem system;
        private const string subject = "Oferta vencida";

        public ExpiredCommandHandler(ICatalogBusiness catalogBusiness, IUnitOfWork unitOfWork, IInvoiceDispersionRepository invoiceDispersionRepository, INotificationRepository notificationRepository,
            ISystem system)
        {
            this.catalogBusiness = catalogBusiness;
            this.unitOfWork = unitOfWork;
            this.invoiceDispersionRepository = invoiceDispersionRepository;
            this.notificationRepository = notificationRepository;
            this.system = system;
        }

        public async Task<ErrorOr<bool>> Handle(ExpiredCommand command, CancellationToken cancellationToken)
        {
            var userId = this.system.User.Id;
            var dateNow = ExtensionFormat.DateTimeCO();
            var invoices = await this.invoiceDispersionRepository.GetAllDefeatedAsync(dateNow, CatalogCode_InvoiceDispersionStatus.PendingPurchase);
            if (!invoices.Any())
            {
                return false;
            }
            List<Notification> notification = new();
            List<InvoiceDispersion> tempInvoiceDispersion = new();
            foreach (var invoicebuyer in invoices)
            {
                var offerNumber = Convert.ToString(invoicebuyer.OfferNumber);
                await SendEmailAdminRejectedTimeAsync(offerNumber);
                notification.Add(new Notification(Guid.NewGuid(), subject, GetErrorDescription(MessageCodes.EmailExpiredOffer, offerNumber), true, ExtensionFormat.DateTimeCO(), userId, ExtensionFormat.DateTimeCO(), userId));
                tempInvoiceDispersion.Add(GetInvoiceDispersion(invoicebuyer));
            }
            await this.notificationRepository.AddAsync(notification);
            await this.invoiceDispersionRepository.UpdateAsync(tempInvoiceDispersion);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task SendEmailAdminRejectedTimeAsync(string numeroOferta)
        {
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToAdminRejectedByTime);
            Dictionary<string, string> replacements = new()
            {
                { "{{numero_oferta}}", numeroOferta },
                { "{{year}}", ExtensionFormat.DateTimeCO().Year.ToString() }
            };
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, replacements);
            var emailAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.EmailAdmin);
            await emainBusiness.SendAsync(emailAdmin.Descripton, subject, templateAdminWithData);
        }

        private InvoiceDispersion GetInvoiceDispersion(InvoiceDispersion dataQuery)
        {
            return new InvoiceDispersion(dataQuery.Id, dataQuery.OfferNumber, dataQuery.BuyerId, dataQuery.SellerId, dataQuery.PayerId, dataQuery.PurchaseDate, dataQuery.EndDate,
                dataQuery.TransactionNumber, dataQuery.InvoiceNumber, dataQuery.Division, dataQuery.Rate, dataQuery.OperationDays, dataQuery.CurrentValue, dataQuery.FutureValue, dataQuery.Reallocation,
                dataQuery.NewMoney, CatalogCode_InvoiceDispersionStatus.Defeated, dataQuery.ExpirationDate, dataQuery.NumberReminder, dataQuery.LastReminder, true, dataQuery.OperationDate, dataQuery.ExpirationDate,
                dataQuery.ParentTransaction, ExtensionFormat.DateTimeCO(), dataQuery.PayerId);
        }
    }
}