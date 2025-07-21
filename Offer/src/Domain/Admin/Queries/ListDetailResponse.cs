///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using System.Text.Json.Serialization;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Offer.Domain.Admin.Queries
{
    public class ListDetailResponse
    {
        public int Nro { get; set; }
        public int NroOffer { get; set; }
        public int TrxId { get; set; }
        public string NameBuyer { get; set; }
        public Guid StatusID { get; set; }
        public string Status { get; set; }

        public string TimeLeft { get; set; }
        public int TimeLeftOrder { get; set; }
        public long CurrentValue { get; set; }
        public long FutureValue { get; set; }
        public string Rate { get; set; }

        [JsonIgnore]
        public DateTime ExpirationDate { get; set; }

        [JsonIgnore]
        public decimal RateAux { get; set; }
    }
}