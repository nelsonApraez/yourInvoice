///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.HistoricalStates
{
    public class HistoricalState : AggregateRoot
    {
        public HistoricalState(Guid id, Guid statusId, Guid offerId, Guid? invoiceDispersionId, bool status, DateTime createdOn, Guid? createdBy, DateTime modifiedOn, Guid? modifiedBy)
        {
            Id = id;
            StatusId = statusId;
            OfferId = offerId;
            InvoiceDispersionId = invoiceDispersionId;
            Status = status;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
        }

        public HistoricalState()
        {
        }

        public Guid StatusId { get; private set; }

        public Guid? OfferId { get; private set; }

        public Guid? InvoiceDispersionId { get; private set; }

        public Offer Offers { get; set; }
    }
}