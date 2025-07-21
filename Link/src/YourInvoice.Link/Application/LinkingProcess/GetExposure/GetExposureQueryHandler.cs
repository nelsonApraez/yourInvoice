///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Offer.Domain;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Link.Application.LinkingProcess.GetExposure
{
    public class GetExposureQueryHandler : IRequestHandler<GetExposureQuery, ErrorOr<GetExposureResponse>>
    {
        private readonly IExposureInformationRepository _repository;
        private readonly ISystem system;

        public GetExposureQueryHandler(IExposureInformationRepository repository, ISystem system)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public async Task<ErrorOr<GetExposureResponse>> Handle(GetExposureQuery query, CancellationToken cancellationToken)
        {
            var idCurrentUser = query?.idGeneralInformation ?? Guid.Empty;
            if (Guid.Empty == idCurrentUser)
            {
                return Error.Validation(MessageCodes.MessageNoExistsCurrentUser, GetErrorDescription(MessageCodes.MessageNoExistsCurrentUser));
            }
            var result = await _repository.GetExposureAsync(idCurrentUser);
            return result;
        }
    }
}