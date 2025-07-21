///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Notifications
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllAsync();

        Task<Notification> GetByIdAsync(Notification id);

        Task<bool> ExistsAsync(Notification id);

        void Update(Notification notification);

        void Delete(Notification notification);

        Task<bool> AddNotificationAsync(Notification notification);

        Task<bool> AddAsync(List<Notification> notification);
    }
}