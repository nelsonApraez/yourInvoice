///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.EventNotifications
{
    public interface IEventNotificationsRepository
    {
        Task<bool> AddAsync(EventNotification eventNotification);

        void Update(EventNotification eventNotification);

        void Delete(Guid eventNotificationId);

        Task NullyfyAsync(Guid offerId);

        Task<bool> AddAsync(List<EventNotification> eventNotifications);
    }
}