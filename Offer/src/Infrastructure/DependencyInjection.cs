///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using yourInvoice.Offer.Application.Data;
using yourInvoice.Offer.Domain.Cufes;
using yourInvoice.Offer.Domain.DianFyMFiles;
using yourInvoice.Offer.Domain.Documents;
using yourInvoice.Offer.Domain.EventNotifications;
using yourInvoice.Offer.Domain.HistoricalStates;
using yourInvoice.Offer.Domain.InvoiceDispersions;
using yourInvoice.Offer.Domain.InvoiceEvents;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.MoneyTransfers;
using yourInvoice.Offer.Domain.Notifications;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.OperationFiles;
using yourInvoice.Offer.Domain.Payers;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Offer.Infrastructure.Persistence;
using yourInvoice.Offer.Infrastructure.Persistence.Repositories;

namespace yourInvoice.Offer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            services.AddScoped<IApplicationDbContext>(sp =>
                    sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IUnitOfWork>(sp =>
                    sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IPayerRepository, PayerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IEventNotificationsRepository, EventNotificationsRepository>();
            services.AddScoped<IInvoiceEventRepository, InvoiceEventRepository>();
            services.AddScoped<IMoneyTransferRepository, MoneyTransferRepository>();
            services.AddScoped<IOperationFileRepository, OperationFileRepository>();
            services.AddScoped<IInvoiceDispersionRepository, InvoiceDispersionRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IHistoricalStatesRepository, HistoricalStatesRepository>();
            services.AddScoped<ICufeRepository, CufeRepository>();
            services.AddScoped<IDianFyMFileRepository, DianFyMFileRepository>();
            return services;
        }
    }
}