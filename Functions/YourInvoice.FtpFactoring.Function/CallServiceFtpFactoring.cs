///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using yourInvoice.FtpFactoring.Function.Business;

namespace yourInvoice.FtpFactoring.Function
{
    public class CallServiceFtpFactoring
    {
        private readonly ILogger _logger;
        private readonly ICallServiceBusiness business;

        public CallServiceFtpFactoring(ICallServiceBusiness business, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CallServiceFtpFactoring>();
            this.business = business;
        }

        [Function("FunctionCallServiceFtpFactoring")]
        public async Task Run([TimerTrigger("%ScheduleTriggerTime%")] TimerInfo myTimer)
        {
            if (myTimer.ScheduleStatus is not null)
            {
                try
                {
                    _logger.LogInformation($"INICIO PROCESO : {DateTime.Now}");
                    await this.business.GetFileFtpAsync();
                    await this.business.GetFileAsync();
                    await this.business.GetReminderAsync();
                    await this.business.GetExpiredAsync();
                    await this.business.GetFileFtpDianAsync();
                    await this.business.ValidationPostProcessDianAsync();
                    _logger.LogInformation($"FINAL PROCESO : {myTimer.ScheduleStatus.Next}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("HAY UN FALLO AL EJECUTAR EL LLAMADO DE LOS SERVICIOS " + ex.Message);
                }
            }
        }
    }
}