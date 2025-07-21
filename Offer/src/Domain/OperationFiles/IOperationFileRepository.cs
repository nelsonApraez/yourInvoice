///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Domain.OperationFiles
{
    public interface IOperationFileRepository
    {
        Task<List<OperationFile>> GetAllAsync();

        Task<OperationFile> GetByIdAsync(OperationFile id);

        Task<bool> ExistsAsync(OperationFile id);

        void Add(OperationFile operationFile);

        void Update(OperationFile operationFile);

        void Delete(OperationFile operationFile);

        Task<bool> AddAsync(List<OperationFile> operationFiles);

        Task<IEnumerable<OperationFile>> GetFilesWithoutProcessAsync();

        Task<bool> UpdateStateToProcessAsync(IEnumerable<OperationFile> operationFilesId);

        Task<bool> UpdateStartDateAsync(IEnumerable<OperationFile> operationFilesId);
    }
}