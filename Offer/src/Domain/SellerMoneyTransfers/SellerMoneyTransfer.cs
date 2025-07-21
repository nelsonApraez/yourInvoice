///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Offer.Domain.SellerMoneyTransfers
{
    public sealed class SellerMoneyTransfer : AggregateRoot
    {
        public SellerMoneyTransfer()
        {
        }

        public SellerMoneyTransfer(Guid id, Guid userId, string name, Guid documentTypeId, string documentNumber, Guid bankId,
            Guid accountTypeId, string accountNumber, bool status, DateTime createdOn, Guid createdBy, DateTime modifiedOn, Guid modifiedBy)
        {
            Id = id;
            UserId = userId;
            Name = name;
            DocumentTypeId = documentTypeId;
            DocumentNumber = documentNumber;
            BankId = bankId;
            AccountTypeId = accountTypeId;
            AccountNumber = accountNumber;
            Status = status;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
        }

        public Guid UserId { get; private set; }

        public string? Name { get; private set; }

        public Guid? DocumentTypeId { get; private set; }

        public string? DocumentNumber { get; private set; }

        public Guid? BankId { get; private set; }

        public Guid? AccountTypeId { get; private set; }

        public string? AccountNumber { get; private set; }

        public User User { get; private set; }
    }
}