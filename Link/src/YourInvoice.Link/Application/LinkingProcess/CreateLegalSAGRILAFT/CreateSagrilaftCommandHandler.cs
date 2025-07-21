
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalSAGRILAFT
{
    public class CreateSagrilaftCommandHandler : IRequestHandler<CreateSagrilaftCommand, ErrorOr<bool>>
    {
        private readonly ILegalSAGRILAFTRepository repository;
        private readonly IUnitOfWorkLink unitOfWork;

        public CreateSagrilaftCommandHandler(ILegalSAGRILAFTRepository repository, IUnitOfWorkLink unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(CreateSagrilaftCommand command, CancellationToken cancellationToken)
        {
            var idCurrentUser = command?.CreateSagrilaft?.FirstOrDefault()?.Id_GeneralInformation ?? Guid.Empty;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var exist = await this.repository.ExistsSagrilaftAsync(idCurrentUser);
            if (exist)
            {
                return Error.Validation(MessageCodes.MessageExistsInformation, GetErrorDescription(MessageCodes.MessageExistsInformation, "información de exposición"));
            }
            var sagrilaft = GetDataSagrilaft(command, idCurrentUser);
            await this.repository.CreateSagrilaftAsync(sagrilaft);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private IEnumerable<LegalSAGRILAFT> GetDataSagrilaft(CreateSagrilaftCommand command, Guid idCurrentUser)
        {
            var Sagrilaft = new List<LegalSAGRILAFT>();
            command.CreateSagrilaft.ToList().ForEach(x =>
            {
                Sagrilaft.Add(new LegalSAGRILAFT(
                id: Guid.NewGuid(),
                id_LegalGeneralInformation: idCurrentUser,
                questionIdentifier: x.QuestionIdentifier,
                responseIdentifier: x.ResponseIdentifier,
                responseDetail: x.ResponseDetail,
                completed: x.Completed,
                statusId: null,
                statusDate: ExtensionFormat.DateTimeCO()
                ));
            });
            return Sagrilaft;
        }
    }
}