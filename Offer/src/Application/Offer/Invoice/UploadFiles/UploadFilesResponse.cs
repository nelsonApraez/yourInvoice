///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.UploadFiles
{
    public class UploadFilesResponse
    {
        public Guid OfferId { get; set; }
        public List<Tuple<string, string>> FilesRejected { get; set; }
        public string ScanTotalProgress { get; set; }
        public string StorageTotalProgress { get; set; }
        public string ValidationBusinessTotalProgress { get; set; }
        public string Status { get; set; }
    }
}