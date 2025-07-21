///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Common.Integration.FtpFiles;
using yourInvoice.Common.Integration.Storage;
using yourInvoice.Offer.Domain.Primitives;
using yourInvoice.Offer.Domain.DianFyMFiles;
using yourInvoice.Common.Business.CatalogModule;

namespace yourInvoice.Offer.Application.DianFyM.GetFileFtpDian
{
    public sealed class GetFileFtpDianQueryHandler : IRequestHandler<GetFileFtpDianQuery, ErrorOr<IEnumerable<string>>>
    {
        private readonly IFtp ftp;
        private readonly IStorage storage;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDianFyMFileRepository repository;

        private string pathFileStorage = "storage/{0}/radian/";
        private const string typeFileBusiness = "exitoso_";
        private const string typeFileSucess = "rechazos_";
        private const string typeFileFal = "fal_";
        private const string typeFile = ".csv";

        public GetFileFtpDianQueryHandler(IFtp ftp, IStorage storage, IUnitOfWork unitOfWork, IDianFyMFileRepository repository)
        {
            this.ftp = ftp ?? throw new ArgumentNullException(nameof(ftp));
            this.storage = storage;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public async Task<ErrorOr<IEnumerable<string>>> Handle(GetFileFtpDianQuery query, CancellationToken cancellationToken)
        {
            var pathNameFiles = await ftp.GetNameAllFilesDianFyMDirectoryAsync();
            var pathNameFilesCon = pathNameFiles?.Where(c => (c.ToLowerInvariant().Contains(typeFileBusiness) ||
                                                             c.ToLowerInvariant().Contains(typeFileSucess) ||
                                                             c.ToLowerInvariant().Contains(typeFileFal)) && c.ToLowerInvariant().Contains(typeFile)).ToList();
            var nameFiles = await ProcessFileAsync(pathNameFilesCon, cancellationToken);
            return nameFiles;
        }

        private async Task<List<string>> ProcessFileAsync(List<string> nameFiles, CancellationToken cancellationToken)
        {
            var offerPendingProcess = await this.repository.GetOfferAwitEndProcessDianAsync(CatalogCode_InvoiceStatus.WaitValidationDian);
            if (!nameFiles.Any() || !offerPendingProcess.Any())
            {
                return new List<string>();
            }
            var dianFileFyM = new List<DianFyMFile>();
            var nameAllFiles = new List<string>();
            var userTemp = Guid.NewGuid();
            bool deleteFileFtp = true;
            var nameFilesNoDuplicate = RemoveNameFileDuplicate(nameFiles);
            foreach (var nameFile in nameFilesNoDuplicate)
            {
                var canProcess = offerPendingProcess.Any(c => nameFile.Contains(c));
                if (!canProcess)
                {
                    continue;
                }
                var file = await ftp.GetFileAsync(nameFile, deleteFileFtp);
                if (file is null || file.Length <= 0)
                {
                    continue;
                }
                int offer = GetOfferNumber(nameFile);
                pathFileStorage = string.Format(pathFileStorage, offer);
                await storage.UploadAsync(file, pathFileStorage + nameFile);
                dianFileFyM.Add(new DianFyMFile(Guid.NewGuid(), offer, nameFile, description: string.Empty, status: true, pathFileStorage, countRegisterFile: 0, ExtensionFormat.DateTimeCO(), userTemp, ExtensionFormat.DateTimeCO(), userTemp));
                nameAllFiles.Add(nameFile);
            }
            if (!dianFileFyM.Any())
            {
                return new List<string>();
            }
            await repository.AddAsync(dianFileFyM);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return nameAllFiles;
        }

        private int GetOfferNumber(string nameFile)
        {
            if (string.IsNullOrEmpty(nameFile))
            {
                return 0;
            }
            var offerNumber = nameFile.Split('_')[2];
            int number = 0;
            int.TryParse(offerNumber, out number);
            return number;
        }

        private List<string> RemoveNameFileDuplicate(List<string> nameFiles)
        {
            var nameFilesNoDuplicate = new List<string>();
            foreach (var nameFile in nameFiles)
            {
                string name = nameFile.Substring(nameFile.LastIndexOf('/') + 1);
                int offer = GetOfferNumber(name);
                if (!nameFilesNoDuplicate.Exists(a => a.Contains($"_{offer}_")))
                {
                    nameFilesNoDuplicate.Add(name);
                }
            }
            return nameFilesNoDuplicate;
        }
    }
}