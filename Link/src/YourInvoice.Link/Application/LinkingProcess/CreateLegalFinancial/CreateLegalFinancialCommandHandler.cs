using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Common.Extension;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalFinancial
{
    public sealed class CreateLegalFinancialCommandHandler : IRequestHandler<CreateLegalFinancialCommand, ErrorOr<bool>>
    {
        private readonly ILegalFinancialInformationRepository financialInformationRepository;
        private readonly IUnitOfWorkLink unitOfWork;


        public CreateLegalFinancialCommandHandler(ILegalFinancialInformationRepository financialInformationRepository, IUnitOfWorkLink unitOfWork)
        {
            this.financialInformationRepository = financialInformationRepository;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<bool>> Handle(CreateLegalFinancialCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.CreateLegalFinancials.Id_LegalGeneralInformation;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var exist = await this.financialInformationRepository.ExistsLegalFinancialAsync(idCurrentUser);
            if (exist)
            {
                return Error.Validation(MessageCodes.MessageExistsInformation, GetErrorDescription(MessageCodes.MessageExistsInformation, "información Financiera"));
            }


            var financial = UtilityBusinessLink.PassDataOriginDestiny(command.CreateLegalFinancials, new LegalFinancialInformation());
            financial.Id = Guid.NewGuid();
            financial.OperationsType = ConvertGuidListToString(command.CreateLegalFinancials.OperationsTypes);
            financial.Id_LegalGeneralInformation = idCurrentUser;
            financial.CreatedOn = ExtensionFormat.DateTimeCO();
            financial.StatusId = CatalogCode_StatusPreRegister.Pending;
            financial.StatusDate = ExtensionFormat.DateTimeCO();
            await this.financialInformationRepository.CreateLegalFinancialAsync(financial);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;

        }

        public static string ConvertGuidListToString(List<Guid?> guidList)
        {
            return string.Join(",", guidList);
        }
    }
}
