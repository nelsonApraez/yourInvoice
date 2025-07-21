///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.OperationFiles
{
    public sealed class OperationFile : AggregateRoot
    {
        public OperationFile(Guid id, string name, string description, DateTime startDate, DateTime endDate, bool status, DateTime createdOn, Guid createdBy, DateTime modifiedOn, Guid modifiedBy)
        {
            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            CreatedOn = createdOn;
            ModifiedBy = modifiedBy;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
        }

        public OperationFile()
        {
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
    }
}