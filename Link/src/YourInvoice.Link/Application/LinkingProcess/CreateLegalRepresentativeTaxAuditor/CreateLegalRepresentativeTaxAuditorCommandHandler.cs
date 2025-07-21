///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalRepresentativeTaxAuditor
{
    public class CreateLegalRepresentativeTaxAuditorCommandHandler : IRequestHandler<CreateLegalRepresentativeTaxAuditorCommand, ErrorOr<bool>>
    {
        private readonly ILegalRepresentativeTaxAuditorRepository repository;
        private readonly IUnitOfWorkLink unitOfWork;

        public CreateLegalRepresentativeTaxAuditorCommandHandler(ILegalRepresentativeTaxAuditorRepository repository, IUnitOfWorkLink unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(CreateLegalRepresentativeTaxAuditorCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.LegalRepresentative.Id_LegalGeneralInformation;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var exist = await this.repository.ExistseLegalRepresentativeTaxAuditorRepositoryAsync(idCurrentUser);
            var legalRepresentative = UtilityBusinessLink.PassDataOriginDestiny(command.LegalRepresentative, new LegalRepresentativeTaxAuditor());
            if (exist)
            {
                await this.repository.UpdateLegalRepresentativeTaxAuditorAsync(legalRepresentative);
                await this.repository.UpdateAccountAsync(legalRepresentative);
                return true;
            }
            legalRepresentative.Id = Guid.NewGuid();
            legalRepresentative.CreatedBy = idCurrentUser;
            legalRepresentative.CreatedOn = ExtensionFormat.DateTimeCO();
            await this.repository.CreateLegalRepresentativeTaxAuditorRepositoryAsync(legalRepresentative);
            await this.repository.UpdateAccountAsync(legalRepresentative);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}