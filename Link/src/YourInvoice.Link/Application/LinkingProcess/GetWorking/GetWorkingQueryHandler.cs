///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using yourInvoice.Offer.Domain;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GetWorking
{
    public class GetWorkingQueryHandler : IRequestHandler<GetWorkingQuery, ErrorOr<GetWorkingResponse>>
    {
        private readonly IWorkingInformationRepository _repository;
        private readonly ISystem system;

        public GetWorkingQueryHandler(IWorkingInformationRepository repository, ISystem system)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public async Task<ErrorOr<GetWorkingResponse>> Handle(GetWorkingQuery query, CancellationToken cancellationToken)
        {
            var idCurrentUser = query.Id_GeneralInformation;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var result = await _repository.GetWorkingAsync(idCurrentUser);
            return result;
        }
    }
}