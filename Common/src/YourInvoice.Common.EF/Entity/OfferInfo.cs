///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class OfferInfo : ModelBase
    {
        public int Consecutive { get; set; }

        public Guid PayerId { get; set; }

        public Guid UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool EndorseLegarAccepted { get; set; }

        public Guid? StatusId { get; set; }
    }
}