///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Primitives;

namespace yourInvoice.Offer.Domain.Payers
{
    public sealed class Payer : AggregateRoot
    {
        public Payer()
        {
        }

        public Payer(Guid id, string nit, string nitDv, string name, string description, string address, string city, bool state, string cityTributa,
           bool stateTributa, string phone, string email, string mailingAddress)
        {
            Id = id;
            Nit = nit;
            NitDv = nitDv;
            Name = name;
            Description = description;
            Address = address;
            City = city;
            State = state;
            CityTributa = cityTributa;
            StateTributa = stateTributa;
            Phone = phone;
            Email = email;
            MailingAddress = mailingAddress;
        }

        public string Nit { get; set; }

        public string NitDv { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public bool? State { get; set; }

        public string CityTributa { get; set; }

        public bool? StateTributa { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string MailingAddress { get; set; }

        public ICollection<Offer> Offers { get; set; }
    }
}