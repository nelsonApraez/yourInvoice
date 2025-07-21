///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.InvoiceEvents
{
    public sealed class InvoiceEvent : AggregateRoot
    {
        public InvoiceEvent()
        {
        }

        public InvoiceEvent(Guid id, Guid invoiceId, bool event030, bool event032, bool event033, bool event036, bool event037,
            bool event06, bool event07, string message, bool status, DateTime createdOn, Guid createdBy, DateTime modifiedOn, Guid modifiedBy, bool? claim)
        {
            Id = id;
            InvoiceId = invoiceId;
            Event030 = event030;
            Event032 = event032;
            Event033 = event033;
            Event036 = event036;
            Event037 = event037;
            Event06 = event06;
            Event07 = event07;
            Message = message;
            Status = status;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
            Claim = claim;
        }

        public Guid InvoiceId { get; private set; }

        public bool? Event030 { get; private set; }

        public bool? Event032 { get; private set; }

        public bool? Event033 { get; private set; }

        public bool? Event036 { get; private set; }

        public bool? Event037 { get; private set; }

        public bool? Event06 { get; set; }

        public bool? Event07 { get; set; }

        public string? Message { get; private set; }

        public bool? Claim { get; set; }
        public Invoice Invoice { get; private set; }
    }
}