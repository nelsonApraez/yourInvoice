///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class EventNotificationInfo : ModelBase
    {
        public Guid OfferId { get; set; }

        public Guid? TypeId { get; set; }

        public string Body { get; set; }

        public string To { get; set; }
    }
}