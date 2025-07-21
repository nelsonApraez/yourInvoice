///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Data.Repositories;

namespace yourInvoice.Common.EF
{
    public static class DependencyInjectionCommon
    {
        public static IServiceCollection AddyourInvoiceCommonEF(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<yourInvoiceCommonDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            services.TryAddScoped<ICatalogRepository, CatalogRepository>();
            services.TryAddScoped<ICatalogItemRepository, CatalogItemRepository>();
            services.TryAddScoped<IDocumentRepository, DocumentRepository>();
            services.TryAddScoped<IEventNotificationRepository, EventNotificationRepository>();
            services.TryAddScoped<IInvoiceEventRepository, InvoiceEventRepository>();
            services.TryAddScoped<IInvoiceRepository, InvoiceRepository>();
            services.TryAddScoped<IMoneyTransferRepository, MoneyTransferRepository>();
            services.TryAddScoped<IOfferRepository, OfferRepository>();
            services.TryAddScoped<IPayerRepository, PayerRepository>();
            services.TryAddScoped<ISellerMoneyTransferRepository, SellerMoneyTransferRepository>();
            services.TryAddScoped<IUserRepository, UserRepository>();
            services.TryAddScoped<IUnitOfWorkCommonEF, UnitOfWorkCommonEF>();
            return services;
        }
    }
}