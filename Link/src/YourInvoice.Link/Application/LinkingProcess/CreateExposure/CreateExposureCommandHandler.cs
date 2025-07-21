///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.CreateExposure
{
    public class CreateExposureCommandHandler : IRequestHandler<CreateExposureCommand, ErrorOr<bool>>
    {
        private readonly IExposureInformationRepository repository;
        private readonly IUnitOfWorkLink unitOfWork;

        public CreateExposureCommandHandler(IExposureInformationRepository repository, IUnitOfWorkLink unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(CreateExposureCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command?.CreateExposures?.FirstOrDefault()?.Id_GeneralInformation ?? Guid.Empty;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var exist = await this.repository.ExistsExposureAsync(idCurrentUser);
            if (exist)
            {
                return Error.Validation(MessageCodes.MessageExistsInformation, GetErrorDescription(MessageCodes.MessageExistsInformation, "información de exposición"));
            }
            var exposure = GetDataExpouse(command, idCurrentUser);
            await this.repository.CreateExposureAsync(exposure);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private IEnumerable<ExposureInformation> GetDataExpouse(CreateExposureCommand command, Guid idCurrentUser)
        {
            var exposure = new List<ExposureInformation>();
            command.CreateExposures.ToList().ForEach(x =>
            {
                exposure.Add(new ExposureInformation(
                id: Guid.NewGuid(),
                id_GeneralInformation: idCurrentUser,
                questionIdentifier: x.QuestionIdentifier,
                responseIdentifier: x.ResponseIdentifier,
                responseDetail: x.ResponseDetail,
                completed: x.Completed,
                statusId: null,
                statusDate: ExtensionFormat.DateTimeCO(),
                declarationOriginFunds: x.DeclarationOriginFunds
                ));
            });
            return exposure;
        }
    }
}