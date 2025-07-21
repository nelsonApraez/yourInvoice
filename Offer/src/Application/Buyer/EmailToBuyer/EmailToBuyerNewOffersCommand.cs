///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.EmailToBuyer
{
    public record EmailToBuyerNewOffersCommand : INotification
    {
        public Dictionary<string, string> AttachData { get; set; }

        public int NumberOffer { get; set; }

        public string EmailBuyerNotification { get; set; }
    }
}