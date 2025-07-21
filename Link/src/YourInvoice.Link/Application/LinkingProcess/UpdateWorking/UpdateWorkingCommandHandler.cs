///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateWorking
{
    public class UpdateWorkingCommandHandler : IRequestHandler<UpdateWorkingCommand, ErrorOr<bool>>
    {
        private readonly IWorkingInformationRepository repository;

        public UpdateWorkingCommandHandler(IWorkingInformationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(UpdateWorkingCommand command, CancellationToken cancellationToken)
        {
            var working = UtilityBusinessLink.PassDataOriginDestiny(command.updateWorking, new WorkingInformation());
            await this.repository.UpdateWorkingAsync(working);
            return true;
        }
    }
}