///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.CreateWorking
{
    public class CreateWorkingCommandHandler : IRequestHandler<CreateWorkingCommand, ErrorOr<bool>>
    {
        private readonly IWorkingInformationRepository repository;
        private readonly IUnitOfWorkLink unitOfWork;

        public CreateWorkingCommandHandler(IWorkingInformationRepository repository, IUnitOfWorkLink unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(CreateWorkingCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command.working.Id_GeneralInformation;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var exist = await this.repository.ExistsWorkingAsync(idCurrentUser);
            if (exist)
            {
                return Error.Validation(MessageCodes.MessageExistsInformation, GetErrorDescription(MessageCodes.MessageExistsInformation, "información laboral"));
            }

            var working = UtilityBusinessLink.PassDataOriginDestiny(command.working, new WorkingInformation());
            working.Id = Guid.NewGuid();
            working.Id_GeneralInformation = idCurrentUser;
            await this.repository.CreateWorkingAsync(working);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}