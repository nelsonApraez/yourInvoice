///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Web.API.Extensions;
using yourInvoice.Offer.Web.API.Middlewares;

namespace yourInvoice.Offer.Web.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<GloblalExceptionHandlingMiddleware>();
            services.AddScoped<ISystem, SystemExtension>();
            return services;
        }
    }
}