///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.FtpFactoring.Function.Business
{
    public interface ICallServiceBusiness
    {
        Task<bool> GetFileAsync();

        Task<bool> GetExpiredAsync();

        Task<bool> GetFileFtpAsync();

        Task<bool> GetReminderAsync();

        Task<bool> ValidationPostProcessDianAsync();

        Task<bool> GetFileFtpDianAsync();
    }
}