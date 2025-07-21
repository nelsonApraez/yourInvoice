///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Offer.Application.User.GetRole
{
    public sealed class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, ErrorOr<List<GetRoleResponse>>>
    {
        private readonly IUserRepository userRepository;
        private readonly ISystem system;

        public GetRoleQueryHandler(IUserRepository userRepository, ISystem system)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public async Task<ErrorOr<List<GetRoleResponse>>> Handle(GetRoleQuery query, CancellationToken cancellationToken)
        {
            var userId = this.system.User.Id;
            var result = await this.userRepository.GetRoleAsync(userId);
            return result;
        }
    }
}