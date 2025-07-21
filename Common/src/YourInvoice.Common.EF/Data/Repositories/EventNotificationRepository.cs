///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.Repositories
{
    public class EventNotificationRepository : Repository<EventNotificationInfo>, IEventNotificationRepository
    {
        private yourInvoiceCommonDbContext _db;

        public EventNotificationRepository(yourInvoiceCommonDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        public async Task<bool> AddEventNotificationAsync(EventNotificationInfo eventNotification)
        {
            await base.AddAsync(eventNotification);

            return true;
        }
    }
}