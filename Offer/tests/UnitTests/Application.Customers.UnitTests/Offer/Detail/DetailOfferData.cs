///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Offers.Queries;

namespace Application.Customer.UnitTest.Offer.Detail
{
    public static class DatailOfferData
    {
        public static DetailOfferResponse GetDetailOfferResponse => new DetailOfferResponse { AmountinvoiceUploadedSuccessfully = 3, beneficiaries = 0, BusinessName = "razon social", OfferId = 300, PayerNit = "900003389", TotalValueOffer = 20000, Status = "EN PROCESO" };

        public static DetailOfferResponse GetDetailOfferResponseNull => null;
    }
}