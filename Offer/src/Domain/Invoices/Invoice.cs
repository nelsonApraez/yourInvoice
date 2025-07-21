///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.Invoices
{
    public class Invoice : AggregateRoot
    {
        public Invoice()
        {
        }

        public Invoice(Guid id, Guid offerId, string number, string zipName, string cufe, Guid statusId, DateTime emitDate, DateTime dueDate,
            decimal total, decimal taxAmount, Guid moneyTypeId, decimal trm, string errorMessage, DateTime? negotiationDate, decimal? negotiationTotal)
        {
            Id = id;
            OfferId = offerId;
            Number = number;
            ZipName = zipName;
            Cufe = cufe;
            StatusId = statusId;
            EmitDate = emitDate;
            DueDate = dueDate;
            Total = total;
            TaxAmount = taxAmount;
            Trm = trm;
            ErrorMessage = errorMessage;
            NegotiationDate = negotiationDate;
            NegotiationTotal = negotiationTotal;
            MoneyTypeId = moneyTypeId;
        }

        public Guid? OfferId { get; private set; }

        public string Number { get; private set; }

        public string ZipName { get; private set; }

        public string Cufe { get; private set; }

        public Guid? StatusId { get; set; }

        public DateTime? EmitDate { get; private set; }

        public DateTime? DueDate { get; private set; }

        public decimal? Total { get; private set; }

        public decimal? TaxAmount { get; set; }

        public Guid? MoneyTypeId { get; private set; }

        public decimal? Trm { get; private set; }

        public string ErrorMessage { get; private set; }

        public DateTime? NegotiationDate { get; private set; }

        public decimal? NegotiationTotal { get; private set; }

        public ICollection<InvoiceEvent> InvoiceEvents { get; set; }

        public Offer Offer { get; set; }
    }
}