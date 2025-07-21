
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.Application.LinkingProcess.GetBank
{
    public class GetBankQueryHandler : IRequestHandler<GetBankQuery, ErrorOr<GetBankResponse>>
    {
        private readonly IBankInformationRepository _repository;

        public GetBankQueryHandler(IBankInformationRepository repository, ISystem system)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GetBankResponse>> Handle(GetBankQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetbankInformationAsync(query.idGeneralInformation);
            return result;
        }
    }
}

