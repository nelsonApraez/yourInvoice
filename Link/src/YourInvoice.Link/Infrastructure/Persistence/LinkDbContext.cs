///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.EF.Data.Access.Mapper;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Common.Primitives;
using yourInvoice.Link.Application.Data;
using yourInvoice.Link.Domain.AccountRoles;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using yourInvoice.Link.Domain.Roles;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.EconomicActivities;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors;
using yourInvoice.Link.Domain.Document;

using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;

namespace yourInvoice.Link.Infrastructure.Persistence
{
    public partial class LinkDbContext : DbContext, ILinkDbContext, IUnitOfWorkLink
    {
        private readonly IPublisher _publisher;

        public LinkDbContext()
        {
        }

        public LinkDbContext(DbContextOptions<LinkDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<EconomicActivity> EconomicActivities { get; set; }

        public DbSet<AccountRole> AccountRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<CatalogItemInfo> CatalogItems { get; set; }

        public DbSet<CatalogInfo> Catalog { get; set; }

        public DbSet<UserInfo> Users { get; set; }

        public DbSet<GeneralInformation> GeneralInformations { get; set; }

        public DbSet<ExposureInformation> ExposureInformations { get; set; }

        public DbSet<WorkingInformation> WorkingInformations { get; set; }

        public DbSet<BankInformation> BankInformations { get; set; }

        public DbSet<FinancialInformation> FinancialInformations { get; set; }

        public DbSet<PersonalReferences> PersonalReference { get; set; }

        public DbSet<SignatureDeclaration> SignatureDeclaration { get; set; }

        public DbSet<LegalGeneralInformation> LegalGeneralInformations { get; set; }

        public DbSet<LegalRepresentativeTaxAuditor> LegalRepresentativeTaxAuditors { get; set; }

        public DbSet<LegalShareholderBoardDirector> LegalShareholdersBoardDirectors { get; set; }

        public DbSet<LegalShareholder> LegalShareholders { get; set; }

        public DbSet<LegalBoardDirector> LegalBoardDirectors { get; set; }

        public DbSet<LegalFinancialInformation> LegalFinancialInformations { get; set; }

        public DbSet<LegalCommercialAndBankReference> LegalCommercialAndBankReferences { get; set; }

        public DbSet<LegalSAGRILAFT> LegalSAGRILAFT { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<LegalSignatureDeclaration> LegalSignatureDeclarations { get; set; }

        public DbSet<LinkStatus> LinkStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogItemInfo>().ToTable(nameof(CatalogItemInfo), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<CatalogInfo>().ToTable(nameof(CatalogInfo), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<UserInfo>().ToTable(nameof(UserInfo), t => t.ExcludeFromMigrations());

            modelBuilder.ApplyConfiguration(new CatalogMapper());
            modelBuilder.ApplyConfiguration(new CatalogItemMapper());
            modelBuilder.ApplyConfiguration(new UserMapper());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkDbContext).Assembly);
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var domainE = ChangeTracker.Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(e => e.GetDomainEvents().Any())
                .SelectMany(e => e.GetDomainEvents());

            var results = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainE)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return results;
        }
    }
}