///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.Extensions.DependencyInjection;
using yourInvoice.Common.Integration.ScanFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Application.Common.Behaviors;

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Resources.NeutralResourcesLanguageAttribute("en")]

namespace yourInvoice.Offer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
            });

            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)
            );

            services.AddScoped<IScanFile, ScanFile>();
            services.AddScoped<IStorage, Storage>();

            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

            return services;
        }
    }
}