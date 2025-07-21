///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Notifications;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            return true;
        }

        public void Delete(Notification notification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Notification id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetByIdAsync(Notification id)
        {
            throw new NotImplementedException();
        }

        public void Update(Notification notification)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddAsync(List<Notification> notification)
        {
            await _context.Notifications.AddRangeAsync(notification);

            return true;
        }
    }
}