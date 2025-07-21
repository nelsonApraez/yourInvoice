///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Entities;

namespace yourInvoice.Common.Persistence
{
    internal class yourInvoiceCommonDbContext : DbContext
    {
        public yourInvoiceCommonDbContext()
        {
        }

        public yourInvoiceCommonDbContext(DbContextOptions<yourInvoiceCommonDbContext> options) : base(options)
        {
        }

        public DbSet<CatalogInfo> Catalogs { get; set; }

        public DbSet<CatalogItemInfo> CatalogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(yourInvoiceCommonDbContext).Assembly);
        }
    }
}