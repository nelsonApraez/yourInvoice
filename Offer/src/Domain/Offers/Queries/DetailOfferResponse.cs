///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Offers.Queries
{
    public class DetailOfferResponse
    {
        public int OfferId { get; set; }
        public string PayerNit { get; set; }
        public string BusinessName { get; set; }
        public int AmountinvoiceUploadedSuccessfully { get; set; }
        public decimal? TotalValueOffer { get; set; }
        public string Status { get; set; }
        public Guid StatusId { get; set; }
        public int beneficiaries { get; set; }
    }
}