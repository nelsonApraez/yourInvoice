///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using yourInvoice.DIAN.Function.Business;

namespace yourInvoice.DIAN.Function
{
    public class ValidationInvoiceIntegrationDian
    {
        private readonly IIntegrationDianBusiness integrationDian;
        private readonly ILogger<ValidationInvoiceIntegrationDian> _logger;
        private const string nameColaCufe = "cufesdian";

        public ValidationInvoiceIntegrationDian(IIntegrationDianBusiness integrationDian, ILogger<ValidationInvoiceIntegrationDian> logger)
        {
            this.integrationDian = integrationDian;
            _logger = logger;
        }

        [Function(nameof(ValidationInvoiceIntegrationDian))]
        public async Task Run([ServiceBusTrigger(nameColaCufe, Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageAction)
        {
            try
            {
                _logger.LogInformation("INICIA PROCESO CON INFORMACION: " + message.Body);
                var resultCreateFile = await this.integrationDian.ValidationProcessDIAN(message, messageAction);
                if (resultCreateFile)
                {
                    _logger.LogInformation("FINALIZO CON EXITO");
                }
                else
                {
                    _logger.LogInformation("FINALIZO CON INCONSISTENCIA SE AGOTO EL TIEMPO DE B�SQUEDA DEL ARCHIVO EN EL FTPS");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await messageAction.DeadLetterMessageAsync(message);
            }
        }
    }
}