///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Common.EF.Entity
{
    public class PayerInfo : ModelBase
    {
        public string Nit { get; set; }

        public string NitDv { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public bool? State { get; set; }

        public string CityTributa { get; set; }

        public bool? StateTributa { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string MailingAddress { get; set; }
    }
}