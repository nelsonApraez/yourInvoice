///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.ValidateDian
{
    public class ValidateDianSend
    {
        public string Nit { get; set; }
        public Guid OfferId { get; set; }
        public int Consecutive { get; set; }
        public List<Envoice> Envoices { get; set; }
    }

    public class Envoice
    {
        public Guid EnvoiceId { get; set; }
        public string CUFE { get; set; }
    }
}