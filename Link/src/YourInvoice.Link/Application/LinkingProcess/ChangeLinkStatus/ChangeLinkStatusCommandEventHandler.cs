///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.Person;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.Users;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus
{
    public class ChangeLinkStatusCommandEventHandler : INotificationHandler<ChangeLinkStatusCommand>
    {
        private readonly ILinkStatusRepository repository;
        private readonly IAccountRepository accountRepository;
        private readonly IUserLinkRepository userRepository;
        private readonly IUnitOfWorkLink unitOfWork;
        private readonly IPersonRepository personRepository;

        public ChangeLinkStatusCommandEventHandler(ILinkStatusRepository repository, IAccountRepository accountRepository, IUserLinkRepository userRepository
            , IUnitOfWorkLink unitOfWork, IPersonRepository personRepository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }

        public async Task Handle(ChangeLinkStatusCommand notification, CancellationToken cancellationToken)
        {
            var idCurrentUser = notification.IdUserLink;
            if (Guid.Empty == idCurrentUser || notification.StatusLinkId == Guid.Empty)
            {
                return;
            }
            var exist = await this.repository.ExistsLinkStatusAsync(idCurrentUser);
            var linkStatus = UtilityBusinessLink.PassDataOriginDestiny(notification, new LinkStatus());
            if (exist)
            {
                await this.repository.UpdateLinkStatusAsync(linkStatus);
                await StatusLinkedApprovedCreatedUserAsync(idCurrentUser, notification.StatusLinkId, cancellationToken);
                await StatusLinkedRejectDeleteUserAsync(idCurrentUser, notification.StatusLinkId);
                return;
            }
            linkStatus.Id = Guid.NewGuid();
            linkStatus.CreatedBy = idCurrentUser;
            linkStatus.CreatedOn = ExtensionFormat.DateTimeCO();
            await this.repository.CreateLinkStatusAsync(linkStatus);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<bool> StatusLinkedApprovedCreatedUserAsync(Guid idAccount, Guid statusLinkId, CancellationToken cancellationToken)
        {
            if (statusLinkId == CatalogCodeLink_LinkStatus.Linked)
            {
                var exists = await this.userRepository.ExistsUserAsync(idAccount);
                if (exists)
                {
                    return false;
                }
                var account = await accountRepository.GetAccountIdAsync(idAccount);
                var dataLink = await GetDataLinkAsync(idAccount, account.PersonTypeId ?? Guid.Empty);
                var user = new UserInfo
                {
                    Id = idAccount,
                    Aadid = string.Empty,
                    IntegrationId = 0,
                    Name = account.Name + " " + account.SecondName + " " + account.LastName + " " + account.SecondLastName,
                    DocumentTypeId = account.DocumentTypeId ?? Guid.Empty,
                    DocumentNumber = account.DocumentNumber,
                    DocumentExpedition = dataLink.DocumentExpedition,
                    Email = account.Email,
                    RoleId = account.RoleId,
                    Phone = account.PhoneNumber,
                    UserTypeId = account.PersonTypeId ?? Guid.Empty,
                    CompanyNit = account.PersonTypeId == CatalogCode_PersonType.Natural ? account.DocumentNumber : account.Nit,
                    CompanyNitDv = account.DigitVerify,
                    Address = dataLink.Address,
                    Company = account.SocialReason?.Length <= 0 ? $"{account.Name} {account.SecondName} {account.LastName} {account.SecondLastName}" : account.SocialReason,
                    City = dataLink.City,
                    CompanyChamberOfCommerceCity = string.Empty,
                    CompanyCommercialRegistrationCity = string.Empty,
                    CompanyCommercialRegistrationNumber = string.Empty,
                    Job = dataLink.Job,
                };
                await this.userRepository.AddAsync(user);
                await this.unitOfWork.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }

        private async Task<GetDataLinkResponse> GetDataLinkAsync(Guid idUserLink, Guid personTypeId)
        {
            if (personTypeId == CatalogCode_PersonType.Natural)
            {
                var dataLink = await this.personRepository.GetDataLinkNaturalUserAsync(idUserLink);
                return dataLink;
            }
            if (personTypeId == CatalogCode_PersonType.Juridica)
            {
                var dataLink = await this.personRepository.GetDataLinkLegalUserAsync(idUserLink);
                return dataLink;
            }
            return new();
        }

        private async Task<bool> StatusLinkedRejectDeleteUserAsync(Guid idAccount, Guid statusLinkId)
        {
            if (statusLinkId == CatalogCodeLink_LinkStatus.Rejected)
            {
                await this.userRepository.DeleteAsync(idAccount);
                return true;
            }
            return false;
        }
    }
}