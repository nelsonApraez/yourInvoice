///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.EventNotifications
{
    public sealed class EventNotification : AggregateRoot
    {
        public EventNotification()
        {
        }

        public EventNotification(Guid id, Guid offerId, Guid typeId, string body, string to,
            bool status, DateTime createdOn, Guid createdBy, DateTime modifiedOn, Guid modifiedBy)
        {
            Id = id;
            OfferId = offerId;
            TypeId = typeId;
            Body = body;
            To = to;
            Status = status;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
        }

        public Guid OfferId { get; private set; }

        public Guid? TypeId { get; private set; }

        public string? Body { get; private set; }

        public string? To { get; private set; }

        public Offer Offer { get; private set; }
    }
}