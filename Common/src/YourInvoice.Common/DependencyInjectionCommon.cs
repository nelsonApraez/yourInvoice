///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.Bus;
using yourInvoice.Common.Integration.Files;
using yourInvoice.Common.Integration.FtpFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Common.Integration.Truora;
using yourInvoice.Common.Integration.ZapSign;
using yourInvoice.Common.Persistence;
using yourInvoice.Common.Persistence.Repositories;

namespace yourInvoice.Common
{
    public static class DependencyInjectionCommon
    {
        public static IServiceCollection AddyourInvoiceCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<yourInvoiceCommonDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            services.TryAddScoped<ICatalogRepository, CatalogRepository>();
            services.TryAddScoped<ICatalogBusiness, CatalogBusiness>();
            services.TryAddScoped<IStorage, Storage>();
            services.TryAddScoped<IFileOperation, FileOperation>();
            services.TryAddScoped<IServiceBus, ServiceBus>();
            services.TryAddScoped<IFtp, Ftp>();
            services.TryAddScoped<IZapsign, Zapsign>();
            services.TryAddScoped<CurrentUserExtension>();
            services.TryAddScoped<ITruora, Truora>();
            return services;
        }
    }
}