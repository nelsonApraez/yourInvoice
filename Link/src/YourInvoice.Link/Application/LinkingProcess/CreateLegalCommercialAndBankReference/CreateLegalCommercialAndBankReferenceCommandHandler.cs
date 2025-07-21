
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalCommercialAndBankReference
{
    public sealed class CreateLegalCommercialAndBankReferenceCommandHandler : IRequestHandler<CreateLegalCommercialAndBankReferenceCommand, ErrorOr<bool>>
    {
        private readonly ILegalCommercialAndBankReferenceRepository commercialAndBankReferenceRepository;
        private readonly IUnitOfWorkLink unitOfWork;


        public CreateLegalCommercialAndBankReferenceCommandHandler(ILegalCommercialAndBankReferenceRepository commercialAndBankReferenceRepository, IUnitOfWorkLink unitOfWork)
        {
            this.commercialAndBankReferenceRepository = commercialAndBankReferenceRepository;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<bool>> Handle(CreateLegalCommercialAndBankReferenceCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.References.Id_LegalGeneralInformation;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var exist = await this.commercialAndBankReferenceRepository.ExistsLegalCommercialAndBankReferenceAsync(idCurrentUser);
            if (exist)
            {
                return Error.Validation(MessageCodes.MessageExistsInformation, GetErrorDescription(MessageCodes.MessageExistsInformation, "referencias comerciales y bancarias"));
            }


            var financial = UtilityBusinessLink.PassDataOriginDestiny(command.References, new LegalCommercialAndBankReference());
            financial.Id = Guid.NewGuid();
            financial.Id_LegalGeneralInformation = idCurrentUser;
            financial.CreatedOn = ExtensionFormat.DateTimeCO();
            financial.StatusId = CatalogCode_StatusPreRegister.Pending;
            financial.StatusDate = ExtensionFormat.DateTimeCO();
            await this.commercialAndBankReferenceRepository.CreateLegalCommercialAndBankReferenceAsync(financial);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
