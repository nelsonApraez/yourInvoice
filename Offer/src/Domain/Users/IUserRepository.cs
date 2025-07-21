///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Offer.Domain.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

        void Update(User user);

        void Delete(Guid userId);

        Task<User> GetByIdAsync(Guid userId);

        Task<ListDataInfo<OfferListResponse>> ListOffersAsync(Guid buyerId, SearchInfo pagination, int timeEnable);

        Task<ListDataInfo<OfferListResponse>> ListOffersHistoryAsync(Guid buyerId, SearchInfo pagination);

        Task<string> GetEmailRoleAsync(Guid roleId);

        Task<string> GetEmailSellerByOfferAsync(int offerId);

        Task<User> GetByEmailAsync(string email);

        Task<List<GetRoleResponse>> GetRoleAsync(Guid userId);

        Task<User> GetUserAsync(string documentNumber, Guid userRoleId);

        Task<Guid?> GetPersonTypeByIdAsync(Guid userId);
    }
}