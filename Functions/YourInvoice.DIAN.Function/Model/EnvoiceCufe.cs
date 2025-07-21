///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.DIAN.Function.Model
{
    public class EnvoiceCufe
    {
        public string Nit { get; set; }

        public Guid OfferId { get; set; }

        public string Consecutive { get; set; }

        public List<Envoice> Envoices { get; set; }
    }
}