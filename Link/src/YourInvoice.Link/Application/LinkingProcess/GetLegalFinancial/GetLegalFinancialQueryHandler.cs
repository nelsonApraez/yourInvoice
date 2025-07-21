
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalFinancial
{
    public class GetLegalFinancialQueryHandler : IRequestHandler<GetLegalFinancialQuery, ErrorOr<GetLegalFinancialResponse>>
    {
        private readonly ILegalFinancialInformationRepository _repository;

        public GetLegalFinancialQueryHandler(ILegalFinancialInformationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GetLegalFinancialResponse>> Handle(GetLegalFinancialQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetLegalFinancialInformationAsync(query.idLegalGeneralInformation);

            result.AccountsForeignCurrencyQuestionId = CatalogCode_LegalFinancial.AccountsForeignCurrencyQuestionId;
            result.OperationsForeignCurrencyQuestionId = CatalogCode_LegalFinancial.OperationsForeignCurrencyQuestionId;
            
            return result;
        }
    }
}
