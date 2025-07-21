///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Offer.Domain;

namespace yourInvoice.Link.Application.LinkingProcess.GetFinancial
{
    public class GetFinancialQueryHandler : IRequestHandler<GetFinancialQuery, ErrorOr<GetFinancialResponse>>
    {
        private readonly IFinancialInformationRepository _repository;

        public GetFinancialQueryHandler(IFinancialInformationRepository repository, ISystem system)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GetFinancialResponse>> Handle(GetFinancialQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetFinancialInformationAsync(query.idGeneralInformation);
            return result;
        }
    }
}

