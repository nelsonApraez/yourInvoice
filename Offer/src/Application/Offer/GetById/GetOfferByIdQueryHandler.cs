///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Offers.Queries;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.GetById
{
    public sealed class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQuery, ErrorOr<GetOfferResponse>>
    {
        private readonly IOfferRepository _repository;

        public GetOfferByIdQueryHandler(IOfferRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GetOfferResponse>> Handle(GetOfferByIdQuery query, CancellationToken cancellationToken)
        {
            if (await _repository.GetByIdWithNamesAsync(query.Id) is not GetOfferResponse getOfferResponse)
            {
                return Error.NotFound(MessageCodes.OfferNotExist, GetErrorDescription(MessageCodes.OfferNotExist));
            }

            return getOfferResponse;
        }
    }
}