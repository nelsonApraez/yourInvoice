///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Accounts.Queries;
using yourInvoice.Link.Domain.Roles;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Link.Application.Accounts.Validity
{
    public class ValidityQueryHandler : IRequestHandler<ValidityQuery, ErrorOr<ValidityResponse>>
    {
        private readonly IRoleRepository roleRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ISystem system;

        public ValidityQueryHandler(IRoleRepository roleRepository, ISystem system, IAccountRepository accountRepository)
        {
            this.roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<ErrorOr<ValidityResponse>> Handle(ValidityQuery query, CancellationToken cancellationToken)
        {
            var userEmail = this.system.User.Email;
            var rol = (await this.roleRepository.GetRoleAsync(userEmail)).FirstOrDefault();
            if (!string.IsNullOrEmpty(rol?.RoleName))
            {
                var userApproved = ApprovedUser(rol);
                return userApproved;
            }
            var account = await this.accountRepository.GetByEmailAsync(userEmail);
            var statusLink = await this.accountRepository.GetStatusLinkAsync(account?.Id ?? Guid.Empty);
            if (statusLink?.StatusLinkId == CatalogCodeLink_LinkStatus.Rejected)
            {
                var userRejectLink = await RejectValidityResponseAsync(account ?? new());
                return userRejectLink;
            }
            var directingPlatform = await ProcessDirectingPlatformAsync(account ?? new());
            return directingPlatform;
        }

        private static ValidityResponse ApprovedUser(GetRoleResponse rol)
        {
            return new ValidityResponse { Process = (int)EnumProccesValidity.Role, Id = rol.RoleId, RolName = rol.RoleName, Name = rol.Name, Email = rol.Email };
        }

        private async Task<ValidityResponse> RejectValidityResponseAsync(Account account)
        {
            bool isLegal = account?.PersonTypeId == CatalogCode_PersonType.Juridica;
            var rolNewUser = await this.roleRepository.GetRoleNewUserAsync(account?.Email ?? string.Empty);
            return new ValidityResponse
            {
                Process = (int)EnumProccesValidity.VinculationReject,
                IsLegal = isLegal,
                RoleId = rolNewUser?.RoleId ?? Guid.Empty,
                RolName = rolNewUser?.RoleName ?? string.Empty,
                Id = account?.StatusId ?? Guid.Empty,
                Email = account?.Email ?? string.Empty,
                Name = account?.Name ?? string.Empty,
                LastName = account?.LastName ?? string.Empty
            };
        }

        private async Task<ValidityResponse> ProcessDirectingPlatformAsync(Account account)
        {
            if (account.StatusId.Equals(CatalogCode_StatusPreRegister.Rejected))
            {
                return new ValidityResponse { Process = (int)EnumProccesValidity.ShowForm, Id = Guid.Empty };
            }
            if (!account.StatusId.Equals(CatalogCode_StatusPreRegister.Approved))
            {
                return new ValidityResponse { Process = (int)EnumProccesValidity.PreRegister, IsLegal = true, Id = account.StatusId ?? Guid.Empty, Email = account.Email ?? string.Empty, SocialReason = account.SocialReason ?? string.Empty, Name = account.Name ?? string.Empty, LastName = account.LastName ?? string.Empty };
            }
            var rolNewUser = await this.roleRepository.GetRoleNewUserAsync(account.Email ?? string.Empty);
            bool isLegal = account.PersonTypeId == CatalogCode_PersonType.Juridica;
            return new ValidityResponse
            {
                Process = (int)EnumProccesValidity.Vinculation,
                IsLegal = isLegal,
                RoleId = rolNewUser.RoleId,
                RolName = rolNewUser.RoleName,
                Id = account.Id,
                Email = account.Email ?? string.Empty,
                SocialReason = account.SocialReason ?? string.Empty,
                Name = account.Name ?? string.Empty,
                LastName = account.LastName ?? string.Empty
            };
        }
    }
}