using yourInvoice.Offer.Domain.Users;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Offer.Web.API.Extensions
{
    public class SystemExtension : ISystem
    {
        public User User { get; set; }

        public void Set(User user)
        {
            this.User = user;
        }
    }
}
