///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Link.Application.LinkingProcess.Common;




namespace yourInvoice.Link.Application.LinkingProcess.CreateFinancial
{
    public sealed class CreateFinancialCommandHandler: IRequestHandler<CreateFinancialCommand, ErrorOr<bool>>
    {

        private readonly IFinancialInformationRepository financialInformationRepository;
        private readonly IUnitOfWorkLink unitOfWork;


        public CreateFinancialCommandHandler(IFinancialInformationRepository financialInformationRepository, IUnitOfWorkLink unitOfWork)
        {
            this.financialInformationRepository = financialInformationRepository;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }

        public async Task<ErrorOr<bool>> Handle(CreateFinancialCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.CreateFinancials.Id_GeneralInformation;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var exist = await this.financialInformationRepository.ExistsFinancialAsync(idCurrentUser);
            if (exist)
            {
                return Error.Validation(MessageCodes.MessageExistsInformation, GetErrorDescription(MessageCodes.MessageExistsInformation, "información Bancaria"));
            }


            var financial = UtilityBusinessLink.PassDataOriginDestiny(command.CreateFinancials, new FinancialInformation());
            financial.Id = Guid.NewGuid();
            financial.Id_GeneralInformation = idCurrentUser;
            financial.CreatedOn = ExtensionFormat.DateTimeCO();
            financial.StatusId = CatalogCode_StatusPreRegister.Pending;
            financial.StatusDate= ExtensionFormat.DateTimeCO();
            await this.financialInformationRepository.CreateFinancialAsync(financial);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;

        }

    }
}
