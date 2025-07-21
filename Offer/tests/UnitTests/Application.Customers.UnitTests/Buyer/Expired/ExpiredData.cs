using yourInvoice.Offer.Domain.InvoiceDispersions;

namespace Application.Customer.UnitTest.Buyer.Expired
{
    public static class ExpiredData
    {
        public static List<InvoiceDispersion> GetInvoiceDispersion => new List<InvoiceDispersion>()
        {
            new InvoiceDispersion (
                                id: Guid.NewGuid(),
                                 offerNumber: 370,
                                buyerId: Guid.NewGuid(),
                                sellerId: Guid.NewGuid(),
                                payerId: Guid.NewGuid(),
                                purchaseDate: DateTime.Now,
                                endDate: DateTime.Now,
                                transactionNumber: 3,
                                invoiceNumber: "FC2020",
                                division: "01",
                                rate: 70,
                                operationDays: 7,
                               currentValue: 2020,
                                futureValue: 500000,
                               reallocation: 'I',
                               newMoney: false,
                                statusId: Guid.NewGuid(),
                                expirationDate: DateTime.Now,
                                numberReminder: 4,
                                lastReminder: DateTime.Now,
                                status: true,
                               operationDate: DateTime.Now,
                                expectedDate: DateTime.Now,
                                parentTransaction: 10,
                                createdOn: DateTime.Now,
                                createdBy: Guid.NewGuid()
                                )
        };

        public static List<InvoiceDispersion> GetInvoiceDispersionEmpty => new List<InvoiceDispersion>();
    }


}