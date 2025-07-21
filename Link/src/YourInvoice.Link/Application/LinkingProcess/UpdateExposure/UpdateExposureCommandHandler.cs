///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateExposure
{
    public class UpdateExposureCommandHandler : IRequestHandler<UpdateExposureCommand, ErrorOr<bool>>
    {
        private readonly IExposureInformationRepository repository;

        public UpdateExposureCommandHandler(IExposureInformationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(UpdateExposureCommand command, CancellationToken cancellationToken)
        {
            var exposures = GetDataExpouse(command);
            foreach (var exposure in exposures)
            {
                await this.repository.UpdateExposureAsync(exposure);
            }
            return true;
        }

        private IEnumerable<ExposureInformation> GetDataExpouse(UpdateExposureCommand command)
        {
            var exposure = new List<ExposureInformation>();
            command.UpdateExposures.ToList().ForEach(x =>
            {
                exposure.Add(new ExposureInformation(
                id: x.Id,
                id_GeneralInformation: x.Id_GeneralInformation,
                questionIdentifier: x.QuestionIdentifier,
                responseIdentifier: x.ResponseIdentifier,
                responseDetail: x.ResponseDetail,
                completed: x.Completed,
                statusId: null,
                statusDate: ExtensionFormat.DateTimeCO(),
                declarationOriginFunds: x.DeclarationOriginFunds));
            });
            return exposure;
        }
    }
}