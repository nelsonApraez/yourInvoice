///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Offers.Queries;

namespace yourInvoice.Offer.Application.Offer.ListAll
{
    public sealed class ListAllOfferQueryHandler : IRequestHandler<ListAllOfferQuery, ErrorOr<ListDataInfo<ListAllOfferResponse>>>
    {
        private readonly IOfferRepository repository;
        private readonly ISystem system;

        public ListAllOfferQueryHandler(IOfferRepository repository, ISystem system)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public async Task<ErrorOr<ListDataInfo<ListAllOfferResponse>>> Handle(ListAllOfferQuery command, CancellationToken cancellationToken)
        {
            var result = await this.repository.ListAllByUserAsync(command.pagination, this.system.User.Id);

            return result ?? new ListDataInfo<ListAllOfferResponse>();
        }
    }
}