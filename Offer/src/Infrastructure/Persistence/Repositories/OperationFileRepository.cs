///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.OperationFiles;

namespace yourInvoice.Offer.Infrastructure.Persistence.Repositories
{
    public class OperationFileRepository : IOperationFileRepository
    {
        private readonly ApplicationDbContext _context;

        public OperationFileRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(OperationFile operationFile)
        {
            throw new NotImplementedException();
        }

        public void Delete(OperationFile operationFile)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(OperationFile id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OperationFile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationFile> GetByIdAsync(OperationFile id)
        {
            throw new NotImplementedException();
        }

        public void Update(OperationFile operationFile)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddAsync(List<OperationFile> operationFiles)
        {
            await _context.OperationFiles.AddRangeAsync(operationFiles);
            return true;
        }

        public async Task<IEnumerable<OperationFile>> GetFilesWithoutProcessAsync()
        {
            var result = await _context.OperationFiles.Where(c => c.Status == null || c.Status == false).ToListAsync();
            return result;
        }

        public async Task<bool> UpdateStartDateAsync(IEnumerable<OperationFile> operationFilesId)
        {
            await _context.OperationFiles.Where(c => operationFilesId.Select(s => s.Id.ToString()).ToList().Contains(c.Id.ToString()))
                                        .ExecuteUpdateAsync(p => p
                                        .SetProperty(u => u.StartDate, ExtensionFormat.DateTimeCO()));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStateToProcessAsync(IEnumerable<OperationFile> operationFilesId)
        {
            await _context.OperationFiles.Where(c => operationFilesId.Select(s => s.Id.ToString()).ToList().Contains(c.Id.ToString()))
                                        .ExecuteUpdateAsync(p => p
                                        .SetProperty(u => u.Status, true)
                                        .SetProperty(u => u.EndDate, ExtensionFormat.DateTimeCO())
                                        .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO()));

            return true;
        }
    }
}