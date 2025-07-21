///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.Extensions.Caching.Memory;

namespace yourInvoice.Common.Entities
{
    public class InvoiceProcessCache
    {
        private IMemoryCache _cache;

        public InvoiceProcessCache(IMemoryCache cache, Guid OfferId)
        {
            _cache = cache;
            this.OfferId = OfferId;
            this.filesRejected = new();
            // Al crear una instancia, intenta obtenerla de la caché si existe
            if (_cache.TryGetValue(OfferId, out InvoiceProcessCache cachedData))
            {
                this.Status = cachedData.Status;
                this.filesRejected = cachedData.filesRejected;
                this.ScanMax = cachedData.ScanMax;
                this.ScanCurrent = cachedData.ScanCurrent;
                this.ScanTotalProgress = cachedData.ScanTotalProgress;
                this.StorageCurrent = cachedData.StorageCurrent;
                this.StorageMax = cachedData.StorageMax;
                this.StorageTotalProgress = cachedData.StorageTotalProgress;
                this.ValidationBusinessCurrent = cachedData.ValidationBusinessCurrent;
                this.ValidationBusinessMax = cachedData.ValidationBusinessMax;
                this.ValidationBusinessTotalProgress = cachedData.ValidationBusinessTotalProgress;
            }
        }

        private void UpdateCache()
        {
            // Almacena la instancia actual en la caché con la OfferId como clave
            _cache.Set(this.OfferId, this);
        }

        public void DeleteCache()
        {
            // Almacena la instancia actual en la caché con la OfferId como clave
            _cache.Remove(this.OfferId);
        }

        public Guid OfferId { get; set; }

        private string status;

        public string Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    // Actualiza la caché cuando el status cambia
                    UpdateCache();
                }
            }
        }

        private List<Tuple<string, string>> filesRejected;

        public List<Tuple<string, string>> FilesRejected
        {
            get => filesRejected;
            set { filesRejected = value; UpdateCache(); }
        }

        private int scanMax;

        public int ScanMax
        {
            get => scanMax;
            set { scanMax = value; UpdateCache(); }
        }

        private int scanCurrent;

        public int ScanCurrent
        {
            get => scanCurrent;
            set { scanCurrent = value; UpdateCache(); }
        }

        public string ScanTotalProgress
        {
            get { return _cache.Get<InvoiceProcessCache>(OfferId).ScanCurrent.ToString() + "/" + _cache.Get<InvoiceProcessCache>(OfferId).ScanMax; }
            set { }
        }

        private int storageMax;

        public int StorageMax
        {
            get => storageMax;
            set { storageMax = value; UpdateCache(); }
        }

        private int storageCurrent;

        public int StorageCurrent
        {
            get => storageCurrent;
            set { storageCurrent = value; UpdateCache(); }
        }

        public string StorageTotalProgress
        {
            get { return _cache.Get<InvoiceProcessCache>(OfferId).StorageCurrent.ToString() + "/" + _cache.Get<InvoiceProcessCache>(OfferId).StorageMax.ToString(); }
            set { }
        }

        private int validationBusinessMax;

        public int ValidationBusinessMax
        {
            get => validationBusinessMax;
            set { validationBusinessMax = value; UpdateCache(); }
        }

        private int validationBusinessCurrent;

        public int ValidationBusinessCurrent
        {
            get => validationBusinessCurrent;
            set { validationBusinessCurrent = value; UpdateCache(); }
        }

        public string ValidationBusinessTotalProgress
        {
            get { return _cache.Get<InvoiceProcessCache>(OfferId).ValidationBusinessCurrent.ToString() + "/" + _cache.Get<InvoiceProcessCache>(OfferId).ValidationBusinessMax.ToString(); }
            set { }
        }
    }
}