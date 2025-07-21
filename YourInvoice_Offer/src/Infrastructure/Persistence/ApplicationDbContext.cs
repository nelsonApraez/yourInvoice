///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Application.Data;
using yourInvoice.Offer.Domain.Cufes;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.EventNotifications;
using yourInvoice.Offer.Domain.HistoricalStates;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Notifications;
using yourInvoice.Offer.Domain.OperationFiles;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.SellerMoneyTransfers;
using yourInvoice.Offer.Domain.Users;

namespace yourInvoice.Offer.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher) : base(options)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public DbSet<Document> Documents { get; set; }

        public DbSet<EventNotification> EventNotifications { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceEvent> InvoiceEvents { get; set; }

        public DbSet<MoneyTransfer> MoneyTransfers { get; set; }

        public DbSet<Domain.Offer> Offers { get; set; }

        public DbSet<Payer> Payers { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<SellerMoneyTransfer> SellerMoneyTransfers { get; set; }

        public DbSet<CatalogItemInfo> CatalogItems { get; set; }

        public DbSet<CatalogInfo> Catalog { get; set; }

        public DbSet<InvoiceDispersion> InvoiceDispersions { get; set; }

        public DbSet<OperationFile> OperationFiles { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<HistoricalState> HistoricalStates { get; set; }

        public DbSet<Cufe> Cufes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Common.yourInvoiceCommonToolkit).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var domainEvents = ChangeTracker.Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(e => e.GetDomainEvents().Any())
                .SelectMany(e => e.GetDomainEvents());

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}