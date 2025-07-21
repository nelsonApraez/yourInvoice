///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Primitives;

namespace yourInvoice.Link.Domain.LinkingProcesses.EconomicActivities
{
    public class EconomicActivity : AggregateRoot
    {
        public EconomicActivity()
        {
        }

        public EconomicActivity(Guid id, string code, string? description, DateTime? createdOn, Guid? createdBy, DateTime? modifiedOn, Guid? modifiedBy, bool? status, Guid? statusId, DateTime? statusDate)
        {
            Id = id;
            Code = code;
            Description = description;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
            Status = status;
            StatusId = statusId;
            StatusDate = statusDate;
        }

        public string Code { get; set; }

        public string? Description { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? StatusDate { get; set; }
    }
}