///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.EventNotifications;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class EventNotificationsRepository : IEventNotificationsRepository
    {
        private readonly ApplicationDbContext _context;

        public EventNotificationsRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddAsync(EventNotification eventNotification)
        {
            await _context.EventNotifications.AddAsync(eventNotification);
            return true;
        }

        public async Task<bool> AddAsync(List<EventNotification> eventNotifications)
        {
            await _context.EventNotifications.AddRangeAsync(eventNotifications);

            return true;
        }

        public void Delete(Guid eventNotificationId)
        {
            throw new NotImplementedException();
        }

        public void Update(EventNotification eventNotification)
        {
            throw new NotImplementedException();
        }

        public async Task NullyfyAsync(Guid offerId)
        {
            await _context.EventNotifications.Where(x => x.OfferId == offerId).ExecuteDeleteAsync();
        }
    }
}