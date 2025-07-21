///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.DianFyMFiles.Queries;

namespace yourInvoice.Offer.Domain.DianFyMFiles
{
    public interface IDianFyMFileRepository
    {
        Task<bool> AddAsync(List<DianFyMFile> dianFyMFiles);

        Task<IEnumerable<DianFyMFile>> GetDianNemeFilesNoProcessAsync();

        Task<IEnumerable<InvoiceCufeDian>> GetInvoiceAsync(int offer, Guid statusId);

        Task<IEnumerable<string>> GetOfferAwitEndProcessDianAsync(Guid statusId);

        Task<bool> UpdateStartDateAsync(IEnumerable<DianFyMFile> dianFyMFileId);

        Task<bool> UpdateStateToProcessAsync(IEnumerable<DianFyMFile> dianFyMFileId);

        Task<bool> UpdateCountRegisterAsync(Guid id, int countRegister);
    }
}