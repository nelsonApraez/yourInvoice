///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using yourInvoice.FtpFactoring.Function.Business;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.TryAddScoped<ICallServiceBusiness, CallServiceBusiness>();
    })
    .Build();

host.Run();