///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Application.Accounts.Approve.EmailApprove;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.Application.Accounts.Approve
{
    public class ApproveAccountCommandHandler : IRequestHandler<ApproveAccountCommand, ErrorOr<bool>>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IGeneralInformationRepository generalInformationRepository;
        private readonly ILegalGeneralInformationRepository legalgeneralInformationRepository;
        private readonly IMediator mediator;
        private readonly IUnitOfWorkLink unitOfWork;

        public ApproveAccountCommandHandler(IAccountRepository accountRepository, IGeneralInformationRepository generalInformationRepository, IMediator mediator, 
            ILegalGeneralInformationRepository legalgeneralInformationRepository, IUnitOfWorkLink unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.generalInformationRepository = generalInformationRepository;
            this.mediator = mediator;
            this.legalgeneralInformationRepository = legalgeneralInformationRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<bool>> Handle(ApproveAccountCommand command, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccountIdAsync(command.accountId);

            if (account is null || account?.Email?.Length <= 0)
            {
                return false;
            }

            if (account.PersonTypeId == CatalogCode_PersonType.Natural)
            {
                await SaveInDBVinculationNatural(account);
            }

            if (account.PersonTypeId == CatalogCode_PersonType.Juridica)
            {
                await SaveInDBVinculationJuridico(account);
            }

            await this.accountRepository.UpdateStatusAsync(account.Id, CatalogCode_StatusPreRegister.Approved, account.Id, timeHour: 0);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await this.mediator.Publish(new EmailApproveCommand { EmailApprove = account.Email ?? string.Empty });

            return true;
        }

        private Task SaveInDBVinculationNatural(Account account)
        {
            GeneralInformation generalInformation = new GeneralInformation(
                account.Id,
                account.Name,
                account.SecondName,
                account.LastName,
                account.SecondLastName,
                account.DocumentTypeId ?? Guid.Empty,
                account.DocumentNumber,
                null,
                null,
                null,
                null,
                account.Email,
                account.PhoneNumber,
                account.MobileNumber,
                null,
                null,
                "",
                null,
                null,
                null,
                "",
                CatalogCode_FormStatus.InProgress,
                CatalogCode_FormStatus.InProgress,
                CatalogCodeLink_LinkStatus.InProcess,
                ExtensionFormat.DateTimeCO(),
                ExtensionFormat.DateTimeCO()
            );

            generalInformationRepository.Add(generalInformation);
            return Task.CompletedTask;
        }

        private Task SaveInDBVinculationJuridico(Account account)
        {
            LegalGeneralInformation generalInformation = new LegalGeneralInformation(
                account.Id,
                account.Nit,
                account.DigitVerify,
                account.SocialReason,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                account.Email,
                null,
                account.PhoneNumber,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                CatalogCode_FormStatus.InProgress,
                CatalogCodeLink_LinkStatus.InProcess,
                ExtensionFormat.DateTimeCO(),
                ExtensionFormat.DateTimeCO()
            );

            legalgeneralInformationRepository.Add(generalInformation);
            return Task.CompletedTask;
        }
    }
}