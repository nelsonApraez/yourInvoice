///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.EventNotifications;
using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.SellerMoneyTransfers;

namespace yourInvoice.Offer.Application.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<Document> Documents { get; set; }

        public DbSet<EventNotification> EventNotifications { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceEvent> InvoiceEvents { get; set; }

        public DbSet<MoneyTransfer> MoneyTransfers { get; set; }

        public DbSet<Domain.Offer> Offers { get; set; }

        public DbSet<Payer> Payers { get; set; }

        public DbSet<Domain.Users.User> Users { get; set; }

        public DbSet<SellerMoneyTransfer> SellerMoneyTransfers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}