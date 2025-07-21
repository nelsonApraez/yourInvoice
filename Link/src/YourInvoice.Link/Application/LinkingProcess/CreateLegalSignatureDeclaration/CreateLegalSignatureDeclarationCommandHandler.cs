///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalSignatureDeclaration
{
    public class CreateLegalSignatureDeclarationCommandHandler : IRequestHandler<CreateLegalSignatureDeclarationCommand, ErrorOr<bool>>
    {
        private readonly ILegalSignatureDeclarationRepository repository;
        private readonly IUnitOfWorkLink unitOfWork;

        public CreateLegalSignatureDeclarationCommandHandler(ILegalSignatureDeclarationRepository repository, IUnitOfWorkLink unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(CreateLegalSignatureDeclarationCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.LegalSignatureDeclaration.Id_LegalGeneralInformation;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var legalSignatureDeclaration = UtilityBusinessLink.PassDataOriginDestiny(command.LegalSignatureDeclaration, new LegalSignatureDeclaration());
            var exist = await this.repository.ExistsLegalSignatureDeclarationAsync(idCurrentUser);
            if (exist)
            {
                await this.repository.UpdateLegalSignatureDeclarationAsync(legalSignatureDeclaration);
                return true;
            }
            legalSignatureDeclaration.Id = Guid.NewGuid();
            legalSignatureDeclaration.CreatedBy = idCurrentUser;
            legalSignatureDeclaration.CreatedOn = ExtensionFormat.DateTimeCO();
            legalSignatureDeclaration.StatusId = CatalogCode_StatusVinculation.Pending;
            await this.repository.CreateLegalSignatureDeclarationAsync(legalSignatureDeclaration);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}