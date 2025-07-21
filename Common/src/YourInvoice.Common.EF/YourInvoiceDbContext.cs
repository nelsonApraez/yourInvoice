///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using yourInvoice.Common.EF.Entity;

namespace yourInvoice.Common.EF
{
    public class yourInvoiceCommonDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public virtual DbSet<CatalogInfo> Catalogs { get; set; }
        public virtual DbSet<CatalogItemInfo> CatalogItems { get; set; }
        public virtual DbSet<DocumentInfo> Documents { get; set; }
        public virtual DbSet<EventNotificationInfo> EventNotifications { get; set; }
        public virtual DbSet<InvoiceEventInfo> IncoiceEvents { get; set; }
        public virtual DbSet<InvoiceInfo> Invoices { get; set; }
        public virtual DbSet<MoneyTransferInfo> MoneyTransfers { get; set; }
        public virtual DbSet<OfferInfo> Offers { get; set; }
        public virtual DbSet<PayerInfo> Payers { get; set; }
        public virtual DbSet<UserInfo> Users { get; set; }
        public virtual DbSet<SellerMoneyTransferInfo> MoneySellers { get; set; }

        public yourInvoiceCommonDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public yourInvoiceCommonDbContext(DbContextOptions<yourInvoiceCommonDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogInfo>();
            modelBuilder.Entity<CatalogItemInfo>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(yourInvoiceCommonDbContext).Assembly);
        }
    }
}