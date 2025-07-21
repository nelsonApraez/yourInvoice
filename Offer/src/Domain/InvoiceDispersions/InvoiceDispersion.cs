///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.InvoiceDispersions
{
    public sealed class InvoiceDispersion : AggregateRoot
    {
        public InvoiceDispersion(Guid id, int offerNumber, Guid buyerId, Guid sellerId, Guid payerId, DateTime purchaseDate, DateTime endDate, int transactionNumber, string invoiceNumber, string division,
                                decimal rate, int operationDays, long currentValue, long futureValue, char reallocation, bool newMoney, Guid statusId, DateTime expirationDate, int numberReminder,
                                DateTime lastReminder, bool status, DateTime operationDate, DateTime expectedDate, int parentTransaction, DateTime createdOn, Guid createdBy)
        {
            Id = id;
            OfferNumber = offerNumber;
            BuyerId = buyerId;
            SellerId = sellerId;
            PayerId = payerId;
            PurchaseDate = purchaseDate;
            EndDate = endDate;
            TransactionNumber = transactionNumber;
            InvoiceNumber = invoiceNumber;
            Division = division;
            Rate = rate;
            OperationDays = operationDays;
            CurrentValue = currentValue;
            FutureValue = futureValue;
            Reallocation = reallocation;
            NewMoney = newMoney;
            StatusId = statusId;
            ExpirationDate = expirationDate;
            NumberReminder = numberReminder;
            LastReminder = lastReminder;
            Status = status;
            OperationDate = operationDate;
            ExpectedDate = expectedDate;
            ParentTransaction = parentTransaction;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }

        private InvoiceDispersion()
        {
        }

        public Guid Id { get; private set; }
        public int OfferNumber { get; private set; }
        public Guid BuyerId { get; private set; }
        public Guid SellerId { get; private set; }
        public Guid PayerId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int TransactionNumber { get; private set; }
        public string InvoiceNumber { get; private set; }
        public string Division { get; private set; }
        public decimal Rate { get; private set; }
        public int OperationDays { get; private set; }
        public long CurrentValue { get; private set; }
        public long FutureValue { get; private set; }
        public char Reallocation { get; private set; }
        public bool NewMoney { get; private set; }
        public Guid StatusId { get; set; }
        public DateTime ExpirationDate { get; private set; }
        public int NumberReminder { get; private set; }
        public DateTime LastReminder { get; private set; }
        public DateTime OperationDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int ParentTransaction { get; set; }
    }
}