///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.HistoricalStates.Add
{
    public class AddHistoricalCommand : INotification
    {
        public int NumberOffer { get; set; }
        public Guid StatusId { get; set; }
        public Guid OfferId { get; set; }
        public List<Guid> InvoiceDispersionId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PayerId { get; set; }
    }
}