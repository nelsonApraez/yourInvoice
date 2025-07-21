///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.Offers.Queries
{
    public record GetOfferResponse
    (
        string payerNit,
        string payerName,
        string sellerName,
        string status,
        Guid statusId,
        int countInvoices,
        decimal? total,
        int consecutive
    );
}