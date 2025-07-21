///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalGeneralInformation
{
    public class CreateLegalGeneralInformationCommandHandler : IRequestHandler<CreateLegalGeneralInformationCommand, ErrorOr<bool>>
    {
        private readonly ILegalGeneralInformationRepository repository;
        private readonly IUnitOfWorkLink unitOfWork;

        public CreateLegalGeneralInformationCommandHandler(ILegalGeneralInformationRepository repository, IUnitOfWorkLink unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(CreateLegalGeneralInformationCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.LegalGeneral.Id;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var isLegal = await this.repository.ExistsAccountLegalAsync(idCurrentUser, CatalogCode_PersonType.Juridica);
            if (!isLegal)
            {
                return Error.Validation(MessageCodes.MessageNotIsLegal, GetErrorDescription(MessageCodes.MessageNotIsLegal));
            }
            var exist = await this.repository.ExistseLegalGeneralInformationAsync(idCurrentUser);
            var legalGeneral = UtilityBusinessLink.PassDataOriginDestiny(command.LegalGeneral, new LegalGeneralInformation());
            legalGeneral.Id = idCurrentUser;
            legalGeneral.CreatedBy = idCurrentUser;
            legalGeneral.CreatedOn = ExtensionFormat.DateTimeCO();
            if (exist)
            {
                await this.repository.UpdateLegalGeneralInformationAsync(legalGeneral);
                return true;
            }
            legalGeneral.StatusId = CatalogCode_StatusVinculation.Pending;
            await this.repository.CreateLegalGeneralInformationAsync(legalGeneral);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}