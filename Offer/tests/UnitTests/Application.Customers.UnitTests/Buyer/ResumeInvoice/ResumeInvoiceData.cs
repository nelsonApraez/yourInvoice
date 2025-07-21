///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.InvoiceDispersions.Queries;

namespace Application.Customer.UnitTest.Buyer.ResumeInvoice
{
    public static class ResumeInvoiceData
    {
        public static ListDataInfo<ResumeInvoiceResponse> GetResumeInvoiceResponse => new ListDataInfo<ResumeInvoiceResponse>
        {
            Count = 1,
            Data = new List<ResumeInvoiceResponse>
              {
                  new ResumeInvoiceResponse
                  {
                        CurrentValue = 1000,
                        ExpirationDate = "12/12/2012",
                        FutureValue = 20000,
                  }
              }
        };

        public static SearchInfo GeSearchInfo => new SearchInfo();

        public static ListDataInfo<ResumeInvoiceResponse> GetResumeInvoiceResponseEmpty => new ListDataInfo<ResumeInvoiceResponse>();

        public static int NumberOffer => 370;

        public static Guid UserId => Guid.Empty;
    }
}