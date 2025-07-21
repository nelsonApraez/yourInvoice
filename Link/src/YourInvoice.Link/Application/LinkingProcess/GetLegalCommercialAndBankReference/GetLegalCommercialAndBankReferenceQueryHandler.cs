
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalCommercialAndBankReference
{
    public class GetLegalCommercialAndBankReferenceQueryHandler : IRequestHandler<GetLegalCommercialAndBankReferenceQuery, ErrorOr<LegalCommercialAndBankReferenceResponse>>
    {
        private readonly ILegalCommercialAndBankReferenceRepository _repository;

        public GetLegalCommercialAndBankReferenceQueryHandler(ILegalCommercialAndBankReferenceRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<LegalCommercialAndBankReferenceResponse>> Handle(GetLegalCommercialAndBankReferenceQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetLegalCommercialAndBankReferenceAsync(query.idLegalGeneralInformation);

            return result;
        }
    }
}
