///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalSAGRILAFT
{
    public class UpdateSagrilaftCommandHandler : IRequestHandler<UpdateSagrilaftCommand, ErrorOr<bool>>
    {
        private readonly ILegalSAGRILAFTRepository repository;

        public UpdateSagrilaftCommandHandler(ILegalSAGRILAFTRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(UpdateSagrilaftCommand command, CancellationToken cancellationToken)
        {
            var sagrilafts = GetDataSagrilaft(command);
            foreach (var sagrilaft in sagrilafts)
            {
                await this.repository.UpdateSagrilaftAsync(sagrilaft);
            }
            return true;
        }

        private IEnumerable<LegalSAGRILAFT> GetDataSagrilaft(UpdateSagrilaftCommand command)
        {
            var sagrilaft = new List<LegalSAGRILAFT>();
            command.UpdateSagrilaft.ToList().ForEach(x =>
            {
                sagrilaft.Add(new LegalSAGRILAFT(
                id: x.Id,
                id_LegalGeneralInformation: x.Id_GeneralInformation,
                questionIdentifier: x.QuestionIdentifier,
                responseIdentifier: x.ResponseIdentifier,
                responseDetail: x.ResponseDetail,
                completed: x.Completed,
                statusId: null,
                statusDate: ExtensionFormat.DateTimeCO()));
            });
            return sagrilaft;
        }
    }
}