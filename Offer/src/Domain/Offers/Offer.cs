///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.EventNotifications;
using yourInvoice.Offer.Domain.HistoricalStates;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Offer.Domain
{
    public sealed class Offer : AggregateRoot
    {
        public Offer()
        {
        }

        public Offer(Guid id, Guid payerId, Guid userId, DateTime? startDate, DateTime? endDate, string endorseLegarAccepted, Guid statusId)
        {
            Id = id;
            PayerId = payerId;
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
            EndorseLegarAccepted = endorseLegarAccepted;
            StatusId = statusId;
        }

        public int Consecutive { get; private set; }

        public Guid PayerId { get; private set; }

        public Guid UserId { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public string EndorseLegarAccepted { get; private set; }

        public Guid? StatusId { get; set; }

        public ICollection<Document> Document { get; set; } = new List<Document>();

        public ICollection<EventNotification> EventNotifications { get; set; } = new List<EventNotification>();

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        public ICollection<MoneyTransfer> MoneyTransfers { get; set; } = new List<MoneyTransfer>();

        public Payer Payer { get; set; }

        public User User { get; set; }

        public ICollection<HistoricalState> HistoricalStates { get; set; } = new List<HistoricalState>();
    }
}