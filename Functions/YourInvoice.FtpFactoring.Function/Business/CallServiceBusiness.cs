///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.Extensions.Logging;
using System.Text;

namespace yourInvoice.FtpFactoring.Function.Business
{
    public class CallServiceBusiness : ICallServiceBusiness
    {
        private string hostUrl = string.Empty;
        private readonly ILogger _logger;
        private const string mediaType = "application/json";

        public CallServiceBusiness(ILoggerFactory log)
        {
            _logger = log.CreateLogger<CallServiceFtpFactoring>();
            hostUrl = Environment.GetEnvironmentVariable("HostCallService") ?? string.Empty;
        }

        public async Task<bool> GetFileFtpAsync()
        {
            if (string.IsNullOrEmpty(hostUrl))
            {
                _logger.LogWarning($"No existe url {hostUrl} del host para el servicio api/buyer/getfileftp");
                return false;
            }

            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(hostUrl + "api/buyer/getfileftp", new StringContent("", Encoding.UTF8, mediaType));
            }

            return true;
        }

        public async Task<bool> GetFileAsync()
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(hostUrl + "api/buyer/process/file", new StringContent("", Encoding.UTF8, mediaType));
            }

            return true;
        }

        public async Task<bool> GetReminderAsync()
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(hostUrl + "api/buyer/reminder", new StringContent("", Encoding.UTF8, mediaType));
            }

            return true;
        }

        public async Task<bool> GetExpiredAsync()
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(hostUrl + "api/buyer/review/expired", new StringContent("", Encoding.UTF8, mediaType));
            }

            return true;
        }

        public async Task<bool> GetFileFtpDianAsync()
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(hostUrl + "api/dian/getfileftpdian", new StringContent("", Encoding.UTF8, mediaType));
            }
            return true;
        }

        public async Task<bool> ValidationPostProcessDianAsync()
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(hostUrl + "api/dian/process/filedian", new StringContent("", Encoding.UTF8, mediaType));
            }
            return true;
        }
    }
}