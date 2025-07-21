///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Payers.Common;
using yourInvoice.Offer.Domain.Payers;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Payers.GetByNit
{
    public sealed class GetPayerByNitQueryHandler : IRequestHandler<GetPayerByNitQuery, ErrorOr<IReadOnlyList<PayerResponse>>>
    {
        private readonly IPayerRepository _repository;

        public GetPayerByNitQueryHandler(IPayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<IReadOnlyList<PayerResponse>>> Handle(GetPayerByNitQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(query.nit))
            {
                return Error.Validation(GetErrorDescription(MessageCodes.ParameterEmpty, "Nit"));
            }

            IReadOnlyList<Payer> payers = await _repository.GetAllPayerByNitAsync(query.nit);

            var response = payers.Select(s => new PayerResponse(
                 s.Id,
                 s.Nit ?? string.Empty,
                 s.NitDv ?? string.Empty,
                 s.Name ?? string.Empty,
                 s.Description ?? string.Empty
                 )).ToList();

            return response;
        }
    }
}