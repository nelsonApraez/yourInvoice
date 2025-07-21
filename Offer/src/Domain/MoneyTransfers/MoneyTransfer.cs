///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.MoneyTransfers
{
    public sealed class MoneyTransfer : AggregateRoot
    {
        public MoneyTransfer()
        {
        }

        public MoneyTransfer(Guid id, Guid offerId, Guid documentTypeId, string documentNumber, Guid bankId, Guid accountTypeId, string accountNumber
            , decimal total, string name, Guid personTypeId)
        {
            Id = id;
            OfferId = offerId;
            DocumentTypeId = documentTypeId;
            DocumentNumber = documentNumber;
            AccountNumber = accountNumber;
            BankId = bankId;
            AccountTypeId = accountTypeId;
            Total = total;
            Name = name;
            PersonTypeId = personTypeId;
        }

        public Guid OfferId { get; private set; }

        public string Name { get; private set; }

        public Guid? PersonTypeId { get; set; }

        public Guid? DocumentTypeId { get; private set; }

        public string DocumentNumber { get; private set; }

        public Guid? BankId { get; private set; }

        public Guid? AccountTypeId { get; private set; }

        public string AccountNumber { get; private set; }

        public decimal? Total { get; private set; }

        public Offer Offer { get; set; }
    }
}