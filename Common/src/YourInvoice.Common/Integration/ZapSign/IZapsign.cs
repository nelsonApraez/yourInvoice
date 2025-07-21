///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Common.Integration.ZapSign
{
    public interface IZapsign
    {
        Task<ZapsignFileResponse> CreateDocAsync(ZapsignFileRequest parameters);

        ZapsignFileResponse CreateDoc(string url, string uri, string token, ZapsignFileRequest parameters);

        Task<ZapsignFileAttachmentResponse> AddAttachmentAsync(string principalDocToken, ZapsignFileAttachmentRequest parameter);

        ZapsignFileAttachmentResponse AddAttachment(string url, string uri, string token, ZapsignFileAttachmentRequest parameter);

        Task<ZapsignFileDetailResponse> GetDetailAsync(string principalDocToken);

        ZapsignFileDetailResponse GetDetail(string url, string uri, string token);
    }
}