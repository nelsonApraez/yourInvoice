///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Offers.Queries
{
    public class ListAllOfferResponse
    {
        public Guid OfferId { get; set; }
        public int NoOffer { get; set; }
        public string DateCreation { get; set; }
        public DateTime? DateCreationOrder { get; set; }
        public string Status { get; set; }
        public Guid StatusId { get; set; }
        public string NamePayer { get; set; }
    }
}