///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Business.CatalogModule;



namespace yourInvoice.Link.Application.LinkingProcess.CreateBank
{
    public sealed class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, ErrorOr<bool>>
    {
        private readonly IBankInformationRepository bankInformationRepository;
        private readonly IUnitOfWorkLink unitOfWork;

        public CreateBankCommandHandler(IBankInformationRepository bankInformationRepository ,IUnitOfWorkLink unitOfWork)
        {
            this.bankInformationRepository = bankInformationRepository;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }

 
        public async Task<ErrorOr<bool>> Handle(CreateBankCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.CreateBanks.Id_GeneralInformation;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var exist = await this.bankInformationRepository.ExistsBankAsync(idCurrentUser);
            if (exist)
            {
                return Error.Validation(MessageCodes.MessageExistsInformation, GetErrorDescription(MessageCodes.MessageExistsInformation, "información Bancaria"));
            }

            var bank = UtilityBusinessLink.PassDataOriginDestiny(command.CreateBanks, new BankInformation());
            bank.Id = Guid.NewGuid();
            bank.Id_GeneralInformation = idCurrentUser;
            bank.CreatedOn = ExtensionFormat.DateTimeCO();
            bank.StatusId = CatalogCode_StatusPreRegister.Pending;
            bank.StatusDate = ExtensionFormat.DateTimeCO();
            await this.bankInformationRepository.CreateBankAsync(bank);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;

        }

    }
}