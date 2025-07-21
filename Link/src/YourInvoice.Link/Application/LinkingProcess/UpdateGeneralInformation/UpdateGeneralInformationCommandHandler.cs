///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateGeneralInformation
{
    public class UpdateGeneralInformationCommandHandler : IRequestHandler<UpdateGeneralInformationCommand, ErrorOr<bool>>
    {
        private readonly IGeneralInformationRepository repository;

        public UpdateGeneralInformationCommandHandler(IGeneralInformationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(UpdateGeneralInformationCommand command, CancellationToken cancellationToken)
        {
            var generalInf = UtilityBusinessLink.PassDataOriginDestiny(command.generalInformation, new GeneralInformation());
            await this.repository.UpdateGeneralInformationAsync(generalInf);
            return true;
        }
    }
}