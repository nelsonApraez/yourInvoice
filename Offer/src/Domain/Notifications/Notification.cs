///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.Notifications
{
    public sealed class Notification : AggregateRoot
    {
        public Notification()
        {
        }

        public Notification(Guid id, string name, string description, bool status, DateTime createdOn, Guid createdBy, DateTime modifiedOn, Guid modifiedBy)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid UserId { get; private set; }
    }
}