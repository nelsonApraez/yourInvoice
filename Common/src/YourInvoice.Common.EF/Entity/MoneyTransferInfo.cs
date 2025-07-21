///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class MoneyTransferInfo : ModelBase
    {
        public Guid OfferId { get; set; }

        public Guid? DocumentTypeId { get; set; }

        public string DocumentNumber { get; set; }

        public Guid? BankId { get; set; }

        public Guid? AccountTypeId { get; set; }

        public string AccountNumber { get; set; }

        public decimal? Total { get; set; }
    }
}