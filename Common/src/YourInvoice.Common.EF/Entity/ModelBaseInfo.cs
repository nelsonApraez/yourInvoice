///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class ModelBase
    {
        public Guid Id { get; set; }

        public bool? Status { get; protected set; } = true;

        public DateTime? CreatedOn { get; protected set; } = DateTime.UtcNow.AddHours(-5);

        public Guid? CreatedBy { get; protected set; }

        public DateTime? ModifiedOn { get; protected set; } = DateTime.UtcNow.AddHours(-5);

        public Guid? ModifiedBy { get; protected set; }
    }
}