
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************


using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Offer.Domain;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFT
{
    public class GetSagrilaftQueryHandler : IRequestHandler<GetSagrilaftQuery, ErrorOr<GetSagrilaftResponse>>
    {
        private readonly ILegalSAGRILAFTRepository _repository;
        private readonly ISystem system;

        public GetSagrilaftQueryHandler(ILegalSAGRILAFTRepository repository, ISystem system)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public async Task<ErrorOr<GetSagrilaftResponse>> Handle(GetSagrilaftQuery query, CancellationToken cancellationToken)
        {
            var idCurrentUser = query?.idLegalGeneralInformation ?? Guid.Empty;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var result = await _repository.GetSagrilaftAsync(idCurrentUser);
            return result;
        }
    }
}