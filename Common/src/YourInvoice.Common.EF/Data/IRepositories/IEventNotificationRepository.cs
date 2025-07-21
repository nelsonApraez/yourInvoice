///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF.Data.IRepositories
{
    public interface IEventNotificationRepository : IRepository<EventNotificationInfo>
    {
        Task<bool> AddEventNotificationAsync(EventNotificationInfo eventNotification);
    }
}