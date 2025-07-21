///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.FtpFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.OperationFiles;
using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Application.Buyer.GetFileFtp
{
    public sealed class GetFileFtpQueryHandler : IRequestHandler<GetFileFtpQuery, ErrorOr<IEnumerable<string>>>
    {
        private readonly IFtp ftp;
        private readonly IStorage storage;
        private readonly IUnitOfWork unitOfWork;
        private readonly IOperationFileRepository repository;
        private readonly ICatalogBusiness catalogBusiness;
        private const string pathFactoringStorage = "/factoring/received/";
        private const string typeFileBusiness = "comp_of";
        private const string typeFile = ".xlsx";

        public GetFileFtpQueryHandler(IFtp ftp, IStorage storage, IUnitOfWork unitOfWork, IOperationFileRepository repository, ICatalogBusiness catalogBusiness)
        {
            this.ftp = ftp ?? throw new ArgumentNullException(nameof(ftp));
            this.storage = storage;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.catalogBusiness = catalogBusiness;
        }

        public async Task<ErrorOr<IEnumerable<string>>> Handle(GetFileFtpQuery query, CancellationToken cancellationToken)
        {
            var pathNameFiles = await ftp.GetNameAllFilesDirectoryAsync();
            var pathNameFilesCon = pathNameFiles?.Where(c => c.ToLowerInvariant().Contains(typeFileBusiness) && c.ToLowerInvariant().Contains(typeFile)).ToList();
            var nameFiles = await ProcessFileAsync(pathNameFilesCon, cancellationToken);
            return nameFiles;
        }

        private async Task<List<string>> ProcessFileAsync(List<string> nameFiles, CancellationToken cancellationToken)
        {
            if (!nameFiles.Any())
            {
                return new List<string>();
            }
            List<OperationFile> operationFiles = new List<OperationFile>();
            List<string> nameAllFiles = new List<string>();
            var userTemp = Guid.NewGuid();
            var purchaseTimeValidity = await catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.TimeEnableOffers);
            int expirationHours = purchaseTimeValidity is null ? 24 : Convert.ToInt32(purchaseTimeValidity.Descripton);
            bool deleteFileFtp = true;
            foreach (var nameFile in nameFiles)
            {
                var file = await ftp.GetFileFactoringAsync(nameFile, deleteFileFtp);
                string name = nameFile.Substring(nameFile.LastIndexOf('/') + 1);
                await storage.UploadAsync(file, pathFactoringStorage + name);
                operationFiles.Add(new OperationFile(Guid.NewGuid(), name, name, ExtensionFormat.DateTimeCO(), ExtensionFormat.DateTimeCO().AddHours(expirationHours), false, ExtensionFormat.DateTimeCO(), userTemp, ExtensionFormat.DateTimeCO(), userTemp));
                nameAllFiles.Add(name);
            }
            if (!operationFiles.Any())
            {
                return new List<string>();
            }
            await repository.AddAsync(operationFiles);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return nameAllFiles;
        }
    }
}