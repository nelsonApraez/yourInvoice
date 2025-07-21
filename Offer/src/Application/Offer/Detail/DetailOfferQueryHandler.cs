///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Http;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain;
using yourInvoice.Offer.Domain.Offers;
using yourInvoice.Offer.Domain.Offers.Queries;

namespace yourInvoice.Offer.Application.Offer.Detail
{
    public sealed class DetailOfferQueryHandler : IRequestHandler<DetailOfferQuery, ErrorOr<DetailOfferResponse>>
    {
        private readonly IOfferRepository repository;
        private readonly ISystem system;
        private readonly IHttpContextAccessor httpContext;
        private const string keySession = "summarydetail";

        public DetailOfferQueryHandler(IOfferRepository repository, ISystem system, IHttpContextAccessor httpContext)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.system = system ?? throw new ArgumentNullException(nameof(system));
            this.httpContext = httpContext;
        }

        public async Task<ErrorOr<DetailOfferResponse>> Handle(DetailOfferQuery command, CancellationToken cancellationToken)
        {
            DetailOfferResponse summaryDetail = this.httpContext.HttpContext.Session.GetObjectFromJson<DetailOfferResponse>(keySession);
            if (summaryDetail is null)
            {
                var id = this.system.User?.Id ?? Guid.Empty;
                if (id == Guid.Empty)
                {
                    return new DetailOfferResponse();
                }
                summaryDetail = await this.repository.DetailAsync(command.offerId, this.system.User.Id);
                this.httpContext.HttpContext.Session.SetObjectAsJson(keySession, summaryDetail);
            }
            return summaryDetail ?? new DetailOfferResponse();
        }
    }
}