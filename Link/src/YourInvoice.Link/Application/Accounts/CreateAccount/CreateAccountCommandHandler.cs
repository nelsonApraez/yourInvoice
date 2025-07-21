///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Link.Domain.AccountRoles;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;

namespace yourInvoice.Link.Application.Accounts.CreateAccount
{
    public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ErrorOr<Guid>>
    {
        private readonly IAccountRoleRepository accountRoleRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IMediator mediator;
        private readonly IUnitOfWorkLink unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICatalogBusiness _catalogBusiness;

        private const string Subject = "Nuevo usuario registrado en yourInvoicedigital";

        public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWorkLink unitOfWork, IAccountRoleRepository accountRoleRepository,
            IUserRepository userRepository, ICatalogBusiness catalogBusiness, IMediator mediator)
        {
            this.accountRepository = accountRepository;
            this.unitOfWork = unitOfWork;
            this.accountRoleRepository = accountRoleRepository;
            _userRepository = userRepository;
            _catalogBusiness = catalogBusiness;
            this.mediator = mediator;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            var Error = await Validations(command);
            if (Error.IsError)
                return Error;

            Account account = await SaveInDB(command, cancellationToken);
            await SendEmail(command);

            await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = account.Id, StatusLinkId = CatalogCodeLink_LinkStatus.InProcess });

            return account.Id;
        }

        private async Task SendEmail(CreateAccountCommand command)
        {
            var templateAdmin = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToAdminPreRegister);
            var templateUser = await _catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToUserPreRegister);

            Dictionary<string, string> replacements = new()
            {
                { "{{customerName}}", $"{command.name} {command.lastName}" },
                { "{{year}}", ExtensionFormat.DateTimeCO().Year.ToString() }
            };

            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, replacements);
            string templateUserWithData = TransformModule.ReplaceTokens(templateUser.Descripton, replacements);

            EmainBusiness emainBusiness = new(_catalogBusiness);
            //email para admin
            var emailAdmin = await _userRepository.GetEmailRoleAsync(CatalogCode_UserRole.Administrator);
            await emainBusiness.SendAsync(emailAdmin, Subject, templateAdminWithData);
            //email para usuario
            await emainBusiness.SendAsync(command.email, Subject, templateUserWithData);
        }

        private async Task<ErrorOr<Guid>> Validations(CreateAccountCommand command)
        {
            if (string.IsNullOrEmpty(command.email))
                return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Correo electrónico"));
            if (await accountRepository.ExistsByEmailAsync(command.email))
                return Error.Validation(MessageCodes.AccountExist, GetErrorDescription(MessageCodes.AccountExist));
            if (string.IsNullOrEmpty(command.name))
                return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Nombre"));
            if (string.IsNullOrEmpty(command.lastName))
                return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Primer apellido"));
            if (string.IsNullOrEmpty(command.name))
                return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Nombre"));
            if (string.IsNullOrEmpty(command.phoneNumber))
                return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Teléfono principal"));
            if (string.IsNullOrEmpty(command.documentNumber))
                return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Documento identidad"));
            if (command.contactById is null)
                return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Contacto por"));

            if (command.personTypeId == CatalogCode_PersonType.Natural)
            {
                if (string.IsNullOrEmpty(command.mobileNumber))
                    return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Teléfono celular"));
            }

            if (command.personTypeId == CatalogCode_PersonType.Juridica)
            {
                if (string.IsNullOrEmpty(command.nit))
                    return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Nit"));
                if (string.IsNullOrEmpty(command.digitVerify))
                    return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Dígito de verificación"));
                if (string.IsNullOrEmpty(command.socialReason))
                    return Error.Validation(MessageCodes.ParameterEmpty, GetErrorDescription(MessageCodes.ParameterEmpty, "Nombre razón social"));
            }

            return new ErrorOr<Guid>();
        }

        private async Task<Account> SaveInDB(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            Guid guid = Guid.NewGuid();

            Account account = new(guid,
                command.personTypeId,
                command.nit,
                command.digitVerify,
                command.socialReason,
                command.name,
                command.secondName,
                command.lastName,
                command.secondLastName,
                command.documentTypeId,
                command.documentNumber,
                command.email,
                command.mobileNumber,
                command.mobileCountryId,
                command.phoneNumber,
                command.phoneCountryId,
                command.contactById, "",
                CatalogCode_StatusPreRegister.Pending,
                ExtensionFormat.DateTimeCO(), 0,
                ExtensionFormat.DateTimeCO());

            accountRepository.Add(account);

            AccountRole role = new(Guid.NewGuid(), account.Id, command.roleId);

            accountRoleRepository.Add(role);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return account;
        }

    }
}