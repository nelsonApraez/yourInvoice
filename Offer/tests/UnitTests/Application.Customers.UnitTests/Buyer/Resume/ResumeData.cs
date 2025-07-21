///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;

namespace Application.Customer.UnitTest.Buyer.Resume
{
    public static class ResumeData
    {
        public static ResumeResponse GetResumeResponse => new ResumeResponse
        {
            CurrentValue = 1000,
            ExpirationDate = DateTime.UtcNow,
            FutureValue = 20000,
            NamePayer = "Nombre del pagador",
            NameSaler = "Nombre del vendedor",
            NroOffer = 370,
            Documents = new Dictionary<string, List<ListDocsResponse>> { { "documentsOffer", null }, { "documentsBuyer", null } }
        };

        public static ResumeResponse GetResumeResponseEmpty => new ResumeResponse()
        {
            Documents = new Dictionary<string, List<ListDocsResponse>>()
        };

        public static int NumberOffer => 370;

        public static Guid UserId => Guid.Empty;
    }
}