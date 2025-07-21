///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using yourInvoice.Common;
using yourInvoice.Common.EF;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.EF.Data.Repositories;
using yourInvoice.DIAN.Function.Business;

var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddyourInvoiceCommon(configuration);
        services.AddyourInvoiceCommonEF(configuration);
        services.TryAddScoped<IIntegrationDianBusiness, IntegrationDianBusiness>();
        services.TryAddScoped<IInvoiceEventRepository, InvoiceEventRepository>();
        services.TryAddScoped<IInvoiceRepository, InvoiceRepository>();
    })
    .Build();

host.Run();