///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.TransformModule;

namespace yourInvoice.Common
{
    public static class yourInvoiceCommonToolkit
    {
        public static IServiceCollection AddyourInvoiceCommonLibrary(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddTransient<ICatalogBusiness, CatalogBusiness>();

            services.AddTransient<TransformModule>();
            return services;
        }
    }
}